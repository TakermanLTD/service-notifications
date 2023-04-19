using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMq.Common.Models;
using System.Net;
using System.Net.Mail;
using System.Text;
using Takerman.MailService.Consumer.HostedServices;
using Takerman.MailService.Models;

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
                    Credentials = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password),
                    UseDefaultCredentials = true
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
            var messageDto = new MailMessageDto()
            {
                From = message.From.Address,
                To = message.To.FirstOrDefault().Address,
                Body = message.Body,
                Subject = message.Subject
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageDto));

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