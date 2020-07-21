namespace Phoenix.Api.Core.Logging.Models
{
    public class NetworkPhoenixMessage : PhoenixMessage
    {
        public string   HttpMethod       { get; set; }
        public int      Duration         { get; set; }
        public int      ResponseCode     { get; set; }
        public string   ResponseCodeText { get; set; }
        public string   ServiceId        { get; set; }
    }
}
