using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMq.Common.Models;
using RabbitMQ.Client;
using System.Net;
using System.Net.Mail;
using System.Text;
using Takerman.MailService.Models;

namespace Takerman.MailService.Consumer.Services
{
    public class MailService : IMailService
    {
        private readonly SmtpConfig _smtpConfig;
        private readonly RabbitMqConfig _rabbitMqConfig;
        private readonly ConnectionFactory _connectionFactory;

        public MailService(IOptions<SmtpConfig> smtpConfig, IOptions<RabbitMqConfig> rabbitMqConfig)
        {
            _smtpConfig = smtpConfig.Value;
            _rabbitMqConfig = rabbitMqConfig.Value;
            _connectionFactory = new ConnectionFactory()
            {
                HostName = _rabbitMqConfig.Hostname,
                UserName = _rabbitMqConfig.Username,
                Password = _rabbitMqConfig.Password,
                Port = _rabbitMqConfig.Port,
                DispatchConsumersAsync = true
            };
        }

        public async Task Send(MailMessage message)
        {
            try
            {
                using (var client = new SmtpClient()
                {
                    Host = _smtpConfig.Host,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Port = _smtpConfig.Port,
                    Credentials = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password),
                    UseDefaultCredentials = false
                })
                {
                    await client.SendMailAsync(message);
                }
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

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    try
                    {
                        channel.QueueDeclare(_rabbitMqConfig.Queue, durable: false, exclusive: false, autoDelete: false);
                        channel.ExchangeDeclare(_rabbitMqConfig.Exchange, ExchangeType.Direct, durable: false, autoDelete: false);

                        channel.BasicPublish(
                            exchange: _rabbitMqConfig.Exchange,
                            routingKey: _rabbitMqConfig.RoutingKey,
                            mandatory: false,
                            basicProperties: null,
                            body: body);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exception while publishing a message", ex);
                    }
                    finally
                    {
                        channel.Close();
                        connection.Close();
                        channel.Dispose();
                        connection.Dispose();
                    }
                }
            }
        }
    }
}