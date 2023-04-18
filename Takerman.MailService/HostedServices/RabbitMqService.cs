using RabbitMq.Common.Models;
using RabbitMQ.Client;

namespace Takerman.MailService.Consumer.HostedServices
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly RabbitMqConfig _rabbitMqConfig;

        public RabbitMqService(RabbitMqConfig rabbitMqConfig)
        {
            _rabbitMqConfig = rabbitMqConfig;
        }

        public IConnection CreateChannel()
        {
            var connection = new ConnectionFactory()
            {
                HostName = _rabbitMqConfig.HostName,
                UserName = _rabbitMqConfig.Username,
                Password = _rabbitMqConfig.Password,
                DispatchConsumersAsync = true
            };

            var channel = connection.CreateConnection();

            return channel;
        }
    }
}