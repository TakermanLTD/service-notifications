using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMq.Common.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Mail;
using System.Text;
using Takerman.MailService.Consumer.HostedServices;
using Takerman.MailService.Consumer.Services;
using Takerman.MailService.Models;

namespace RabbitMq.Consumer.Services
{
    public class ConsumerService : IConsumerService
    {
        private readonly IMailService _mailService;
        private readonly IOptions<RabbitMqConfig> _rabbitMqConfig;
        private readonly ConnectionFactory _connectionFactory;

        public ConsumerService(
            IMailService mailService,
            IOptions<RabbitMqConfig> rabbitMqConfig)
        {
            _mailService = mailService;
            _rabbitMqConfig = rabbitMqConfig;
            _connectionFactory = new ConnectionFactory()
            {
                HostName = _rabbitMqConfig.Value.Hostname,
                UserName = _rabbitMqConfig.Value.Username,
                Password = _rabbitMqConfig.Value.Password,
                Port = _rabbitMqConfig.Value.Port,
                DispatchConsumersAsync = true
            };
        }

        public async Task ReadMessages()
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(_rabbitMqConfig.Value.Queue, durable: false, exclusive: false, autoDelete: false);
                    channel.ExchangeDeclare(_rabbitMqConfig.Value.Exchange, ExchangeType.Direct, durable: false, autoDelete: false);
                    channel.QueueBind(_rabbitMqConfig.Value.Queue, _rabbitMqConfig.Value.Exchange, _rabbitMqConfig.Value.RoutingKey);

                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.Received += async (ch, ea) =>
                    {
                        try
                        {
                            var body = ea.Body.ToArray();

                            var text = Encoding.UTF8.GetString(body);

                            var mailDto = JsonConvert.DeserializeObject<MailMessageDto>(text);

                            var mail = new MailMessage(mailDto.From, mailDto.To)
                            {
                                Subject = mailDto.Subject,
                                Body = mailDto.Body,
                                IsBodyHtml = true
                            };

                            await _mailService.Send(mail);

                            await Task.CompletedTask;

                            channel.BasicAck(ea.DeliveryTag, false);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Failed to consume the request", ex);
                        }
                    };

                    channel.BasicConsume(_rabbitMqConfig.Value.Queue, false, consumer);

                    await Task.CompletedTask;
                }
            }
        }
    }
}