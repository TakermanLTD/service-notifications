﻿namespace Takerman.MailService.Models
{
    public class MailMessageDto
    {
        public string Name { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}