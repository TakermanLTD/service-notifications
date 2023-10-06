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
        private readonly IConnection _connection;
        private readonly IModel _channel;

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
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public async Task ReadMessages()
        {
            try
            {
                _channel.QueueDeclare(_rabbitMqConfig.Value.Queue, durable: false, exclusive: false, autoDelete: false);
                _channel.ExchangeDeclare(_rabbitMqConfig.Value.Exchange, ExchangeType.Direct, durable: false, autoDelete: false);
                _channel.QueueBind(_rabbitMqConfig.Value.Queue, _rabbitMqConfig.Value.Exchange, _rabbitMqConfig.Value.RoutingKey);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.Received += async (ch, ea) =>
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

                    _channel.BasicAck(ea.DeliveryTag, false);

                    await Task.CompletedTask;
                };

                _channel.BasicConsume(_rabbitMqConfig.Value.Queue, false, consumer);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to consume the request", ex);
            }
        }
    }
}