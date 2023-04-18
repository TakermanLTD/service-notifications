using RabbitMQ.Client;

namespace Takerman.MailService.Consumer.HostedServices
{
    public interface IRabbitMqService
    {
        IModel GetModel();

        IConnection CreateChannel();
    }
}