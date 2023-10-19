namespace Takerman.MailService.Queue
{
    public class DeadLetterQueue
    {
        public const string Queue = "deadLetter";
        public const string Exchange = "deadLetterExchange";
        public const string RoutingKey = "deadLetterRouting";
        public static Dictionary<string, object> Args = new Dictionary<string, object>
        {
            {"x-dead-letter-exchange", Exchange},
            {"x-dead-letter-routing-key", "deadLetter"},
            {"x-message-ttl", 60000},
        };
    }
}
