namespace Phoenix.Api.Core.Logging.Models
{
    public class PhoenixMessage
    {
        public string   MachineName      { get; set; }
        public string   ApplicationName  { get; set; }
        public string   AssemblyName     { get; set; }
        public string   MethodName       { get; set; }
        public int      LineNumber       { get; set; }
        public string   Severity         { get; set; }
        public string   MessageText      { get; set; }   
        public string   UserId           { get; set; }
        public string   UserEmail        { get; set; }
        public string   ClientId         { get; set; }
        public string   UserName         { get; set; }
    }
}