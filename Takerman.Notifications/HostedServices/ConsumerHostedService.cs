using Takerman.MailService.Consumer.HostedServices;

namespace RabbitMq.Consumer.HostedServices
{
    public class ConsumerHostedService(IConsumerService _consumerService, ILogger<ConsumerHostedService> _logger) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _consumerService.ReadMessages();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("The notification service has been stopped");
        }
    }
}