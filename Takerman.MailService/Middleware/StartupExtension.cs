using RabbitMq.Common.Models;
using Takerman.MailService.Consumer.Services;

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