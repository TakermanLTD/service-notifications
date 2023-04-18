using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace PersonalArea.Business.Tests
{
    public class MailTests
    {
        [Test]
        [TestCase("contact@takerman.net", "tivanov@takerman.net", null, "d-54deebc5b445469aa54e6812df716301", "b08e97ea-2e76-4809-9e08-0e22ef657d8e")]
        public void Should_SendATestEmailSuccessfully_When_SendEmailMethodIsCalled(string from, string to, string bcc, string templateId, string version)
        {
            var email = new
            {
                from = from,
                to = to,
                bcc = bcc,
                templateId = templateId,
                version = version,
                data = new
                {
                    first_name = "Test",
                    last_name = "Test",
                    not_found_test = "404",
                    fr = "true"
                }
            };

            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(email));

            channel.BasicPublish(exchange: "",
                                 routingKey: "test",
                                 basicProperties: null,
                                 body: body);
            Assert.True(true);
        }
    }
}