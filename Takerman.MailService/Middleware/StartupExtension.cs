using Takerman.MailService.Consumer.HostedServices;
using Takerman.MailService.Consumer.Services;
using RabbitMq.Common.Models;

namespace Takerman.MailService.Consumer.Middleware
{
    public static class StartupExtension
    {
        public static void AddCommonService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqConfig>(configuration.GetSection(nameof(RabbitMqConfig)));
            services.Configure<SmtpConfig>(configuration.GetSection(nameof(SmtpConfig)));
        }

        public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder)
        => applicationBuilder.UseMiddleware<GlobalErrorHandlingMiddleware>();
    }
}