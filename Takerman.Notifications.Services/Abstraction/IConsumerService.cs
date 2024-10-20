namespace Takerman.Notifications.Services.Abstraction
{
    public interface IConsumerService
    {
        Task ReadMessages();
    }
}