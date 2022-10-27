using System.Threading.Tasks;

namespace lab1_pws.Services.Interfaces.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string To, string ToName, string Subject, string Body);
    }
}
