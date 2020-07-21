using System;

namespace Phoenix.Api.Core.RabbitClient.Exceptions
{
    public class MissingRabbitHostException : Exception
    {
        public MissingRabbitHostException()
        {
        }

        public MissingRabbitHostException(string message): base(message)
        {
        }
    }
}