using System.Net.Mail;

namespace Takerman.Notifications.Services.Abstraction
{
    public interface IMailService
    {
        Task Send(MailMessage message);
    }
}