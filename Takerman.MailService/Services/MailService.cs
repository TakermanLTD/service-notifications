using Takerman.MailService.Consumer.HostedServices;
using RabbitMq.Common.Models;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using System.Text;
using Newtonsoft.Json;

namespace Takerman.MailService.Consumer.Services
{
    public class MailService : IMailService
    {
        private readonly SmtpConfig _smtpConfig;
        private readonly RabbitMqConfig _rabbitMqConfig;
        private readonly IRabbitMqService _rabbitMqService;

        public MailService(
            IOptions<SmtpConfig> smtpConfig,
            IOptions<RabbitMqConfig> rabbitMqConfig,
            IRabbitMqService rabbitMqService)
        {
            _smtpConfig = smtpConfig.Value;
            _rabbitMqConfig = rabbitMqConfig.Value;
            _rabbitMqService = rabbitMqService;
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

        public async Task SendToQueue(MailMessage message)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            var channel = _rabbitMqService.GetModel();

            channel.BasicPublish(
                exchange: _rabbitMqConfig.Exchange,
                routingKey: _rabbitMqConfig.RoutingKey,
                mandatory: true,
                basicProperties: null,
                body: body);
        }
    }
}