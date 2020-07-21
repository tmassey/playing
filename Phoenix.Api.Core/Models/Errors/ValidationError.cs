namespace Phoenix.Api.Core.Errors
{
    public class ValidationError : Error
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }

        public ValidationError(string code, string message, string propertyName, string propertyValue)
            : base(code, message)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public new string LogMessage()
        {
            return $"{Code} - {Message}, Property Name: {PropertyName}, Property Value: {PropertyValue}";
        }
    }
}
