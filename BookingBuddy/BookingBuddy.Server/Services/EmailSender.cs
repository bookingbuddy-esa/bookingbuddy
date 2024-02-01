﻿using SendGrid.Helpers.Mail;
using SendGrid;

namespace BookingBuddy.Server.Services
{
    public class EmailSender
    {
        public async Task SendEmail(string subject, string toEmail, string name, string message)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("bookingbuddy.bb@gmail.com", "BookingBuddy");
            //var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress(toEmail, name);
            //var plainTextContent = "and easy to do anywhere, even with C#";
            //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            var response = await client.SendEmailAsync(msg);
        }
    }
}