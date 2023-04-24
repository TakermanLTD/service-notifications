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
    public class ConsumerService : IConsumerService, IDisposable
    {
        private readonly IModel _model;
        private readonly IMailService _mailService;
        private readonly RabbitMqConfig _rabbitMqConfig;
        private readonly IConnection _connection;

        public ConsumerService(
            IRabbitMqService rabbitMqService,
            IMailService mailService,
            IOptions<RabbitMqConfig> rabbitMqConfig)
        {
            _mailService = mailService;
            _rabbitMqConfig = rabbitMqConfig.Value;
            _connection = rabbitMqService.CreateChannel();
            _model = _connection.CreateModel();
            _model.QueueDeclare(_rabbitMqConfig.Queue, durable: true, exclusive: false, autoDelete: false);
            _model.ExchangeDeclare(_rabbitMqConfig.Exchange, ExchangeType.Direct, durable: true, autoDelete: false);
            _model.QueueBind(_rabbitMqConfig.Queue, _rabbitMqConfig.Exchange, _rabbitMqConfig.RoutingKey);
        }

        public async Task ReadMessages()
        {
            var consumer = new AsyncEventingBasicConsumer(_model);
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

                    _model.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to consume the request", ex);
                }
            };
            _model.BasicConsume(_rabbitMqConfig.Queue, true, consumer);
            
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();

            if (_connection.IsOpen)
                _connection.Close();
        }
    }
}