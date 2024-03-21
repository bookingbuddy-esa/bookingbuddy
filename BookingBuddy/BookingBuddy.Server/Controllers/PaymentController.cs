using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
using BookingBuddy.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookingBuddy.Server.Controllers
{
    /// <summary>
    /// Classe que representa o controlador de gestão de pagamentos.
    /// </summary>
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly BookingBuddyServerContext _context;
        private readonly IConfiguration _configuration;
        private static readonly WebSocketWrapper<Payment> WebSocketWrapper = new();

        /// <summary>
        /// Construtor da classe PaymentController.
        /// </summary>
        /// <param name="context">Contexto da base de dados</param>
        /// <param name="userManager">Gestor de utilizadores</param>
        /// <param name="configuration">Configuração da aplicação</param>
        public PaymentController(BookingBuddyServerContext context, UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Método que retorna um pagamento.
        /// </summary>
        /// <param name="paymentId">Identificador do pagamento</param>
        /// <returns>Retorna: 
        /// - O pagamento, caso exista um pagamento com o identificador inserido.
        /// - 400 Bad Request, caso o identificador inserido seja uma string nula ou vazia.
        /// - 404 Not Found, caso não exista nenhum pagamento com o id inserido.
        /// </returns>
        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPayment(string paymentId)
        {
            if (string.IsNullOrEmpty(paymentId))
            {
                return BadRequest("O identificador do pagamento é inválido.");
            }

            var payment = await _context.Payment.FindAsync(paymentId);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }


        /// <summary>
        /// Método que representa o endpoint de webhook para notificações de pagamentos.
        /// </summary>
        /// <returns>Retorna:
        /// 200 Ok, se o pagamento para promover uma propriedade for efetuado com sucesso
        /// 401 Unauthorized, se a chave não for a correta
        /// 400 Bad Request, se os dados da resposta do pagamento estiverem incompletos ou inválidos
        /// 404 Not Found, se não houver nenhum pagamento com o identificador da resposta do pagamento
        /// 500 Status Code, caso ocorra alguma exceção
        /// </returns>
        [HttpGet("webhook")]
        public async Task<IActionResult> Webhook([FromQuery] PaymentResponse paymentResponse)
        {
            var key = paymentResponse.key;
            var orderId = paymentResponse.orderId;
            var amount = paymentResponse.amount;
            var requestId = paymentResponse.requestId;
            var paymentDatetime = paymentResponse.payment_datetime;

            if (key != _configuration.GetSection("PhishingKey").Value!)
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(amount) || string.IsNullOrEmpty(requestId) ||
                string.IsNullOrEmpty(paymentDatetime))
            {
                return BadRequest("Dados de pagamento incompletos ou inválidos.");
            }

            var payment = await _context.Payment.FindAsync(requestId);
            if (payment == null)
            {
                return NotFound();
            }

            if (payment.Status == "Paid") return Ok();
            try
            {
                payment.Status = "Paid";
                OrderBase? order = (await _context.Order.FindAsync(orderId))?.Type.ToUpper() switch
                {
                    "PROMOTE" => await _context.PromoteOrder.FindAsync(orderId),
                    "BOOKING" => await _context.BookingOrder.FindAsync(orderId),
                    "GROUP-BOOKING" => await _context.GroupBookingOrder.FindAsync(orderId),
                    _ => null
                };

                if (order == null) return NotFound();

                var blockDates = new BlockedDate
                {
                    PropertyId = order.PropertyId,
                    Start = order.StartDate.ToString("yyyy-MM-dd"),
                    End = order.EndDate.ToString("yyyy-MM-dd")
                };

                if (order is BookingOrder)
                {
                    _context.BlockedDate.Add(blockDates);
                }

                if (order is GroupBookingOrder groupBookingOrder)
                {
                    var group = await _context.Groups.FindAsync(groupBookingOrder.GroupId);
                    var groupPayment = _context.GroupOrderPayment.FirstOrDefault(p => p.PaymentId == requestId);
                    groupBookingOrder.PaidByIds.Add(groupPayment?.ApplicationUserId ?? "Unknown User");
                    if (group != null && groupBookingOrder.PaidByIds.Count == group.MembersId.Count)
                    {
                        order.State = OrderState.Paid;
                        _context.BlockedDate.Add(blockDates);
                    }
                }
                else
                {
                    order.State = OrderState.Paid;
                }

                await _context.SaveChangesAsync();
                await WebSocketWrapper.NotifyAllAsync(payment);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro: {ex.Message}");
            }
        }

        /// <summary>
        /// Método que representa o endpoint de criação de um pagamento.
        /// </summary>
        /// <returns>Retorna:
        /// 401 Unauthorized, caso o utilizador introduzido por parâmetro seja nulo
        /// 400 Bad Request, caso o método de pagamento introduzido por parâmetro não seja um dos dois métodos de pagamento existentes (mbway e multibanco)
        /// ArgumentOutOfRangeException, caso o a quantia monetária introduzida por parâmetro não seja válida
        /// ArgumentException, caso o número de telemóvel introduzido por parâmetro não seja válido
        /// 500 Status Code, caso ocorra alguma exceção
        /// </returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Payment>> CreatePayment(ApplicationUser user, string paymentMethod,
            decimal amount, string phoneNumber)
        {
            try
            {
                if (user == null)
                {
                    return Unauthorized();
                }

                var _httpClient = new HttpClient();
                dynamic requestData = null;
                string endpointUrl = "";

                if (paymentMethod == "mbway")
                {
                    requestData = new
                    {
                        mbWayKey = _configuration.GetSection("MbWayKey").Value!,
                        orderId = Guid.NewGuid().ToString(),
                        amount = amount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                        mobileNumber = phoneNumber,
                        email = user.Email,
                        description = "Pagamento de teste - Booking Buddy"
                    };

                    endpointUrl = "https://ifthenpay.com/api/spg/payment/mbway";

                    // regex com o padrão de um número de telemóvel em Portugal, a começar por 9 seguido por 1, 2, 3 ou 6 e por fim seguido por quaisquer 7 dígitos entre 0 e 9
                    string regexPattern = @"^(9[1236]\d{7})$";

                    Regex regex = new Regex(regexPattern);

                    if (!regex.IsMatch(phoneNumber))
                    {
                        throw new ArgumentException(
                            "O número de telemóvel introduzido é inválido. Tem de começar pelo dígito 9, seguido por 1, 2, 3 ou 6, e com 9 dígitos no total.");
                    }
                }
                else if (paymentMethod == "multibanco")
                {
                    requestData = new
                    {
                        mbKey = _configuration.GetSection("MbKey").Value!,
                        orderId = Guid.NewGuid().ToString(),
                        amount = amount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                        description = "Pagamento de teste - Booking Buddy",
                        url = "https://bookingbuddy.azurewebsites.net/",
                        clientCode = user.Id,
                        clientName = user.Name,
                        clientEmail = user.Email,
                        clientUsername = user.UserName,
                        clientPhone = user.PhoneNumber,
                        expiryDays = 0 // TODO: 24 horas, mas pode ser aumentado
                    };

                    endpointUrl = "https://ifthenpay.com/api/multibanco/reference/sandbox";
                }
                else
                {
                    return BadRequest("Método de pagamento inválido.");
                }

                if (amount < 0.0M || amount > 100000.0M)
                {
                    throw new ArgumentOutOfRangeException(nameof(amount),
                        "A quantia monetária deve estar entre 0.0€ e 100000.0€.");
                }

                /*Console.WriteLine("request to ifthenpay: " + Newtonsoft.Json.JsonConvert.SerializeObject(requestData));
                Console.WriteLine("endpoint: " + endpointUrl);*/

                var jsonContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData),
                    Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(endpointUrl, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Falha a criar pagamento. StatusCode: " + response.StatusCode);
                    return StatusCode((int)response.StatusCode, "Falha a criar pagamento.");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);

                try
                {
                    var newPayment = new Payment
                    {
                        PaymentId = responseJson.RequestId,
                        Method = paymentMethod,
                        Amount = amount,
                        Status = "Pending",
                        CreatedAt = DateTime.Now
                    };

                    _context.Payment.Add(newPayment);
                    await _context.SaveChangesAsync();

                    if (responseJson.Entity != null)
                    {
                        newPayment.Entity = responseJson.Entity;
                        newPayment.Reference = responseJson.Reference;
                        newPayment.ExpiryDate = responseJson.ExpiryDate;
                    }

                    return newPayment;
                }
                catch (Exception ex)
                {
                    return StatusCode(500,
                        $"Ocorreu um erro enquanto o pagamento estava a ser guardado na base de dados: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro: {ex.Message}");
            }
        }

        [NonAction]
        public async Task HandleWebSocketAsync(string paymentId, WebSocket webSocket)
        {
            var payment = await _context.Payment.FindAsync(paymentId);
            await WebSocketWrapper.HandleAsync(payment, webSocket);
        }

        /// <summary>
        /// Classe interna para representar uma resposta de um pagamento.
        /// </summary>
        public class PaymentResponse
        {
            /// <summary>
            /// Representa a chave.
            /// </summary>
            public string key { get; set; }

            /// <summary>
            /// Representa o identificador da reserva.
            /// </summary>
            public string orderId { get; set; }

            /// <summary>
            /// Representa a quantia monetária.
            /// </summary>
            public string amount { get; set; }

            /// <summary>
            /// Representa o identificador do pedido.
            /// </summary>
            public string requestId { get; set; }

            /// <summary>
            /// Representa a data e a hora do pagamento.
            /// </summary>
            public string payment_datetime { get; set; }
        }
    }
}