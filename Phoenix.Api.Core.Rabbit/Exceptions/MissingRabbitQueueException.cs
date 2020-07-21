using System;

namespace Phoenix.Api.Core.RabbitClient.Exceptions
{
    public class MissingRabbitQueueException : Exception
    {
        public MissingRabbitQueueException()
        {
        }
        public MissingRabbitQueueException(string message) : base(message)
        {
        }
    }
}