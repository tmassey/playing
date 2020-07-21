using System;
using Phoenix.Api.Core.Configurators.Interfaces;

namespace Phoenix.Api.Core.Configurators
{
    public class RabbitExchangeConfigurator : IRabbitExchangeConfigurator
    {
        public string ExchangeName { get; set; }
        public string HostName { get; set; } = "localhost";
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; } = 5672;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(ExchangeName)) throw new Exception("ExchangeName is required");
            if (string.IsNullOrWhiteSpace(HostName)) throw new Exception("HostName is required");
            if (string.IsNullOrWhiteSpace(UserName)) throw new Exception("Username is required");
            if (string.IsNullOrWhiteSpace(Password)) throw new Exception("Password is required");
        }
    }

}
