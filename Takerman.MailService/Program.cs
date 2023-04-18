using Takerman.MailService.Consumer.HostedServices;
using Takerman.MailService.Consumer.Middleware;
using Takerman.MailService.Consumer.Services;
using RabbitMq.Consumer.HostedServices;
using RabbitMq.Consumer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCommonService(builder.Configuration);
builder.Services.AddTransient<IRabbitMqService, RabbitMqService>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddSingleton<IConsumerService, ConsumerService>();
builder.Services.AddHostedService<ConsumerHostedService>();

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