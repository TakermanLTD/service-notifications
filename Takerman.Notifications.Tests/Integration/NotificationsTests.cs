using System.Net.Mail;
using Takerman.Notifications.Services.Abstraction;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Takerman.Backups.Tests.Integration
{
    public class NotificationsTests : TestBed<TestFixture>
    {
        private readonly IConsumerService? _consumerService;
        private readonly IMailService? _mailService;

        public NotificationsTests(ITestOutputHelper testOutputHelper, TestFixture fixture)
        : base(testOutputHelper, fixture)
        {
            _consumerService = _fixture.GetService<IConsumerService>(_testOutputHelper);
            _mailService = _fixture.GetService<IMailService>(_testOutputHelper);
        }

        [Fact(Skip = "Build")]
        public void Should_DeleteDatabase_When_ConnectedToTheServer()
        {
            Assert.True(true);
        }

        [Fact(Skip = "Build")]
        public async Task Should_SendATestEmailSuccessfully_When_GmailIsCalled()
        {
            var record = Record.ExceptionAsync(async () =>
            {
                var message = new MailMessage("tivanov@takerman.net", "contact@takerman.net", "Test Subject", "Test body");

                await _mailService.Send(message);
            });

            Assert.Null(record?.Exception);
        }

        [Fact(Skip = "Build")]
        public async Task Should_SendATestEmailSuccessfully_When_RabbitMqIsCalled()
        {
            var record = Record.ExceptionAsync(async () =>
            {
                var message = new MailMessage("tivanov@takerman.net", "contact@takerman.net", "Test Subject", "Test body");

                await _mailService.Send(message);
            });

            Assert.Null(record?.Exception);
        }
    }
}