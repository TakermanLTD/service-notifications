namespace RabbitMq.Common.Models
{
    public class RabbitMqConfig
    {
        public string HostName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Port { get; set; }

        public string Exchange { get; set; }

        public string Queue { get; set; }

        public string RoutingKey { get; set; }
    }
}