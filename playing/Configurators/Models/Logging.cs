using Phoenix.Api.Core.Logging.Models;

namespace playing.Configurators.Models
{
    public class Logging
    {
        public string LoggingExchange { get; set; }
        public string LoggingHost { get; set; }
        public int LoggingPort { get; set; }
        public bool LogToConsole { get; set; }
        public string LoggingUserName { get; set; }
        public string LoggingPassword { get; set; }
        public Severity LogLevel { get; set; }
    }
}