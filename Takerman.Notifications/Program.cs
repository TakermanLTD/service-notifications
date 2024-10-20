using RabbitMq.Consumer.HostedServices;
using RabbitMq.Consumer.Services;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Slack;
using Serilog.Sinks.Slack.Models;
using Takerman.MailService.Consumer.HostedServices;
using Takerman.MailService.Consumer.Middleware;
using Takerman.MailService.Consumer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCommonService(builder.Configuration);
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddSingleton<IConsumerService, ConsumerService>();
builder.Services.AddHostedService<ConsumerHostedService>();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Slack(new SlackSinkOptions
    {
        WebHookUrl = "https://hooks.slack.com/services/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",
        CustomIcon = ":email:",
        Period = TimeSpan.FromSeconds(10),
        ShowDefaultAttachments = false,
        ShowExceptionAttachments = true,
        MinimumLogEventLevel = LogEventLevel.Error,
        PropertyDenyList = ["Level", "SourceContext"]
    })
    .CreateLogger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.AddGlobalErrorHandler();

app.MapControllers();

app.Run();