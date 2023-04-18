namespace Takerman.MailService.Consumer.HostedServices
{
    public interface IConsumerService
    {
        Task ReadMessages();
    }
}