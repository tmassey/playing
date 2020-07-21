using System;

namespace Phoenix.Api.Core.RabbitClient.Models
{
    public class RabbitTransaction
    {
        public Guid message_id { get; set; }

        public string queue_name { get; set; }

        public string routing_key { get; set; }

        public string payload { get; set; }
    }
}