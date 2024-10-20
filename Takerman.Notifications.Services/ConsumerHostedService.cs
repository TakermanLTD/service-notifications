using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Takerman.Notifications.Services.Abstraction;

namespace Takerman.Notifications.Services
{
    public class ConsumerHostedService(IConsumerService _consumerService, ILogger<ConsumerHostedService> _logger) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
                await _consumerService.ReadMessages();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                _logger.LogInformation("The notification service has been stopped");
        }
    }
}