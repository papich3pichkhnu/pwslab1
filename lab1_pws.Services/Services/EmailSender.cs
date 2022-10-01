using lab1_pws.Services.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;

namespace lab1_pws.Services.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly Helpers.Mails.MailSettings _mailSettings;
        public EmailSender(IOptions<Helpers.Mails.MailSettings> options)
        {
            _mailSettings = options.Value;
        }
        public EmailSender()
        {
            
        }
        public async Task SendEmailAsync(string To, string ToName, string Subject, string Body)
        {
            var client = new SendGridClient(_mailSettings.ApiKey);

            string Content = $"Thank you for your message:\n{Body}\nWe will answer on it soon.";

            SendGridMessage message = new()
            {
                From = new EmailAddress(_mailSettings.Email, _mailSettings.DisplayName),
                Subject = Subject,
                PlainTextContent = Content
            };

            message.AddTo(new EmailAddress(To, ToName));

            await client.SendEmailAsync(message);
        }
    }
}
