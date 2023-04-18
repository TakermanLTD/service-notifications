using Takerman.MailService.Consumer.HostedServices;
using RabbitMq.Common.Models;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Takerman.MailService.Consumer.Services
{
    public class MailService : IMailService
    {
        private readonly SmtpConfig _smtpConfig;

        public MailService(IOptions<SmtpConfig> smtpConfig, IRabbitMqService rabbitMqService)
        {
            _smtpConfig = smtpConfig.Value;
        }

        public async Task Send(MailMessage message)
        {
            try
            {
                var client = new SmtpClient()
                {
                    Host = _smtpConfig.Host,
                    EnableSsl = true,
                    Port = _smtpConfig.Port,
                    Credentials = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password)
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