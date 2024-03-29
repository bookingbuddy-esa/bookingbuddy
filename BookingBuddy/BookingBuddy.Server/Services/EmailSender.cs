using SendGrid.Helpers.Mail;
using SendGrid;

namespace BookingBuddy.Server.Services
{
    /// <summary>
    /// Classe que representa o envio de emails.
    /// </summary>
    public abstract class EmailSender
    {
        /// <summary>
        /// Envia um email de acordo com uma template dinâmica do SendGrid.
        /// </summary>
        /// <param name="apiKey">Chave de API do SendGrid.</param>
        /// <param name="templateId">O identificador da template.</param>
        /// <param name="toEmail">O email do destinatário.</param>
        /// <param name="toName">O nome do destinatário.</param>
        /// <param name="data">Os dados a serem enviados.</param>
        public static async Task SendTemplateEmail(string apiKey, string templateId, string toEmail, string toName,
            object data)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("bookingbuddy.bb@gmail.com", "BookingBuddy");
            var to = new EmailAddress(toEmail, toName);
            var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, data);
            await client.SendEmailAsync(msg);
        }
    }
}
