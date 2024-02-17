using System.Net.Mail;

namespace Takerman.MailService.Consumer.Services
{
    public interface IMailService
    {
        Task Send(MailMessage message);
    }
}