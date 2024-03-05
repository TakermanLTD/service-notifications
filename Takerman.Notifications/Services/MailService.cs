using Microsoft.Extensions.Options;
using RabbitMq.Common.Models;
using System.Net;
using System.Net.Mail;

namespace Takerman.MailService.Consumer.Services
{
    public class MailService(IOptions<SmtpConfig> _smtpConfig) : IMailService
    {
        public async Task Send(MailMessage message)
        {
            try
            {
                using var client = new SmtpClient()
                {
                    Host = _smtpConfig.Value.Host,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Port = _smtpConfig.Value.Port,
                    Credentials = new NetworkCredential(_smtpConfig.Value.Username, _smtpConfig.Value.Password),
                    UseDefaultCredentials = false
                };
                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot send email", ex);
            }
        }
    }
}