using Microsoft.Extensions.Options;
using RabbitMq.Common.Models;
using RabbitMQ.Client;

namespace Takerman.MailService.Consumer.HostedServices
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly RabbitMqConfig _rabbitMqConfig;

        public RabbitMqService(IOptions<RabbitMqConfig> rabbitMqConfig)
        {
            _rabbitMqConfig = rabbitMqConfig.Value;
        }

        public IConnection CreateChannel()
        {
            return new ConnectionFactory()
            {
                HostName = _rabbitMqConfig.Hostname,
                UserName = _rabbitMqConfig.Username,
                Password = _rabbitMqConfig.Password,
                Port = _rabbitMqConfig.Port,
                DispatchConsumersAsync = true
            }.CreateConnection();
        }

        public IModel GetModel()
        {
            return CreateChannel().CreateModel();
        }
    }
}