using System.Text;
using System.Web;
using BookingBuddy.Server.Data;
using BookingBuddy.Server.Models;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Construtor da classe PaymentController.
        /// </summary>
        /// <param name="context">Contexto da base de dados</param>
        /// <param name="userManager">Gestor de utilizadores</param>
        /// <param name="configuration">Configuração da aplicação</param>
        public PaymentController(BookingBuddyServerContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPayment(string paymentId)
        {
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
        /// <returns></returns>
        [HttpGet("webhook")]
        public async Task<IActionResult> Webhook([FromQuery] PaymentResponse paymentResponse)
        {
            string key = paymentResponse.key;
            string orderId = paymentResponse.orderId;
            string amount = paymentResponse.amount;
            string requestId = paymentResponse.requestId;
            string paymentDatetime = paymentResponse.payment_datetime;

            Console.WriteLine($"Key: {key}");
            Console.WriteLine($"Order ID: {orderId}");
            Console.WriteLine($"Amount: {amount}");
            Console.WriteLine($"Request ID: {requestId}");
            Console.WriteLine($"Payment Datetime: {paymentDatetime}");

            if (key != _configuration.GetSection("PhishingKey").Value!)
            {
                return Unauthorized();
            }

            var payment = await _context.Payment.FindAsync(requestId);
            if (payment == null)
            {
                return NotFound();
            }

            payment.Status = "Paid";

            try {
                await _context.SaveChangesAsync();
                var order = await _context.Order.FindAsync(orderId);
                if (order != null)
                {
                    order.State = true;
                    await _context.SaveChangesAsync();

                    if (orderId.StartsWith("PROMOTE-"))
                    {
                        var promoteOrder = new PromoteOrder
                        {
                            PromoteOrderId = Guid.NewGuid().ToString(),
                            OrderId = orderId
                        };
                        _context.PromoteOrder.Add(promoteOrder);
                        await _context.SaveChangesAsync();
                    }

                    // TODO: criar BookingOrder se a orderID tiver "BOOKING-" no início
                }
            } catch (Exception ex) {
                return StatusCode(500, $"An error occurred while updating payment status: {ex.Message}");
            }

            return Ok();
        }

        /// <summary>
        /// Método que representa o endpoint de criação de um pagamento.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Payment>> CreatePayment(ApplicationUser user, string paymentMethod, decimal amount, string phoneNumber)
        {
            try
            {
                if(user == null) {
                    return Unauthorized();
                }

                var _httpClient = new HttpClient();
                dynamic requestData = null;
                string endpointUrl = "";

                if(paymentMethod == "mbway")
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

                } else if (paymentMethod == "multibanco")
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
                } else
                {
                    return BadRequest("Invalid payment method.");
                }

                /*Console.WriteLine("request to ifthenpay: " + Newtonsoft.Json.JsonConvert.SerializeObject(requestData));
                Console.WriteLine("endpoint: " + endpointUrl);*/

                var jsonContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(endpointUrl, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Failed to create payment. StatusCode: " + response.StatusCode);
                    return StatusCode((int)response.StatusCode, "Failed to create payment.");
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
                    return StatusCode(500, $"An error occurred while saving payment to database: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

    public class PaymentResponse {
        public string key { get; set; }
        public string orderId { get; set; }
        public string amount { get; set; }
        public string requestId { get; set; }
        public string payment_datetime { get; set; }
    }
}
