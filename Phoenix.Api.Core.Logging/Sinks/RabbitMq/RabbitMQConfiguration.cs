using System;
using RabbitMQ.Client;
using Serilog.Sinks.RabbitMQ;

namespace Phoenix.Api.Core.Logging.Sinks.RabbitMq
{
    public class RabbitMQConfiguration
    {
        public string Hostname = string.Empty;
        public string Username = string.Empty;
        public string Password = string.Empty;
        public string Exchange = string.Empty;
        public string ExchangeType = string.Empty;
        public RabbitMQDeliveryMode DeliveryMode = RabbitMQDeliveryMode.NonDurable;
        public string RouteKey = string.Empty;
        public int Port;
        public string VHost = string.Empty;
        public IProtocol Protocol;
        public ushort Heartbeat;
        public int BatchPostingLimit;
        public TimeSpan Period;
    }
}