using Phoenix.Api.Core.Logging.Enums;

namespace Phoenix.Api.Core.Logging
{
    public abstract class PhoenixLoggerBase
    {
        public Severity LogLevel { get; set; } = Severity.Info;
    }
}
