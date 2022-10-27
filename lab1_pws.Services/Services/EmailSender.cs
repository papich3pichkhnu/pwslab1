using lab1_pws.Services.Interfaces.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace lab1_pws.Services.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly Helpers.Mails.MailSettings _mailSettings;
        public EmailSender(IOptionsMonitor<Helpers.Mails.MailSettings> options)
        {
            _mailSettings = options.CurrentValue;
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
