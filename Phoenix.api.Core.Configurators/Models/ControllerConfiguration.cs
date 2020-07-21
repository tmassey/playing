namespace Phoenix.Api.Core.Configurators.Models
{
    public class ControllerConfiguration
    {
        public string RabbitExchangeName { get; set; }
        public string RabbitHostName { get; set; }
        public int RabbitPort { get; set; }
        public string RabbitUserName { get; set; }
        public string RabbitPassword { get; set; }
        public bool RegisterWithController { get; set; }
        public bool RegisterListener { get; set; }
        public string ControllerExchange { get; set; }
        public int MSTimeout { get; set; }
    }
}