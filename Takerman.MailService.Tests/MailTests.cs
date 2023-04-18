using Microsoft.Extensions.Configuration;
using RabbitMq.Common.Models;
using System.Net.Mail;
using Takerman.MailService.Consumer.HostedServices;
using Takerman.MailService.Consumer.Services;

namespace PersonalArea.Business.Tests
{
    public class MailTests
    {
        private readonly IConfigurationRoot _configuration;
        private readonly RabbitMqConfig _rabbitMqConfig;
        private readonly SmtpConfig _mailConfig;
        private readonly IRabbitMqService _rabbitMqService;
        private readonly MailService _mailService;

        public MailTests()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).Build();
            _rabbitMqConfig = _configuration.GetSection(nameof(RabbitMqConfig)).Get<RabbitMqConfig>();
            _mailConfig = _configuration.GetSection(nameof(SmtpConfig)).Get<SmtpConfig>();
            _rabbitMqService = new RabbitMqService(_rabbitMqConfig);
            _mailService = new MailService(_mailConfig, _rabbitMqService);
        }

        [Test]
        public async Task Should_SendATestEmailSuccessfully_When_SendEmailMethodIsCalled()
        {
            var message = new MailMessage("tivanov@takerman.net", "tanyo@takerman.net", "Test Subject", "Test body");

            await _mailService.Send(message);

            Assert.True(true);
        }
    }
}