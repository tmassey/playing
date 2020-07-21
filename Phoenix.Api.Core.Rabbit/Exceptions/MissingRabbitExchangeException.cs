using System;

namespace Phoenix.Api.Core.RabbitClient.Exceptions
{
    public class MissingRabbitExchangeException : Exception
    {
        public MissingRabbitExchangeException()
        {
        }
        public MissingRabbitExchangeException(string message) : base(message)
        {
        }
    }
}