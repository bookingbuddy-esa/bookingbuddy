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
    public class PaymentController : Controller
    {
        private readonly BookingBuddyServerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Construtor da classe PaymentController.
        /// </summary>
        /// <param name="context">Contexto da base de dados</param>
        /// <param name="userManager">Gestor de utilizadores</param>
        public PaymentController(BookingBuddyServerContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Método que representa o endpoint de webhook para notificações de pagamentos.
        /// </summary>
        /// <returns></returns>
        [HttpGet("webhook")]
        public async Task<IActionResult> Webhook([FromQuery] MbwayResponse mbwayResponse)
        {
            string key = mbwayResponse.key;
            string orderId = mbwayResponse.orderId;
            string amount = mbwayResponse.amount;
            string requestId = mbwayResponse.requestId;
            string paymentDatetime = mbwayResponse.payment_datetime;

            Console.WriteLine($"Key: {key}");
            Console.WriteLine($"Order ID: {orderId}");
            Console.WriteLine($"Amount: {amount}");
            Console.WriteLine($"Request ID: {requestId}");
            Console.WriteLine($"Payment Datetime: {paymentDatetime}");
            return Ok();
        }

        /// <summary>
        /// Método que representa o endpoint de criação de um pagamento.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePayment()
        {
            try
            {
                return Ok("Pagamento criado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        private string GetAmountByPromotionDuration(DateTime startingDate, DateTime endingDate)
        {
            var duration = (endingDate - startingDate).TotalDays;
            // TODO: Calcular o valor do pagamento com base na duração da promoção
            // 1 semana, 1 mês e 1 ano
            return "1.00";
        }
    }

    public class MbwayResponse {
        public string key { get; set; }
        public string orderId { get; set; }
        public string amount { get; set; }
        public string requestId { get; set; }
        public string payment_datetime { get; set; }
    }
}
