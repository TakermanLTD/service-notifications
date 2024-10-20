using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMq.Common.Models;
using Takerman.Notifications.Services;
using Takerman.Notifications.Services.Abstraction;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Backups.Tests
{
    public class TestFixture : TestBedFixture
    {
        protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
            => services
                .Configure<RabbitMqConfig>(configuration.GetSection(nameof(RabbitMqConfig)))
                .Configure<SmtpConfig>(configuration.GetSection(nameof(SmtpConfig)))
                .AddTransient<IMailService, Notifications.Services.MailService>()
                .AddSingleton<IConsumerService, ConsumerService>()
                .AddHostedService<ConsumerHostedService>();

        protected override ValueTask DisposeAsyncCore() => new();

        protected override IEnumerable<TestAppSettings> GetTestAppSettings()
        {
            var result = new List<TestAppSettings>()
            {
                new(){ Filename = "test-appsettings.json", IsOptional = false },
                new(){ Filename = "test-appsettings.Production.json", IsOptional = true }
            };

            return result;
        }
    }
}