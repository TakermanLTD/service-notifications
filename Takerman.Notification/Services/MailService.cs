using Microsoft.Extensions.Options;
using RabbitMq.Common.Models;
using System.Net;
using System.Net.Mail;

namespace Takerman.MailService.Consumer.Services
{
    public class MailService : IMailService
    {
        private readonly SmtpConfig _smtpConfig;

        public MailService(IOptions<SmtpConfig> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }

        public async Task Send(MailMessage message)
        {
            try
            {
                using var client = new SmtpClient()
                {
                    Host = _smtpConfig.Host,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Port = _smtpConfig.Port,
                    Credentials = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password),
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