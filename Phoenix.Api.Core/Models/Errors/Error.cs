namespace Phoenix.Api.Core.Errors
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public Error() { }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string LogMessage()
        {
            return $"{Code} - {Message}";
        }
    }
}
