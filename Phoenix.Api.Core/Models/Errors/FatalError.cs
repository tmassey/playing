using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;

namespace Phoenix.Api.Core.Errors
{
    public class FatalError : Error
    {
        public IList<string> StackTrace { get; set; }

        public FatalError() { }

        public FatalError(string code, string message) : base(code, message)
        {
            StackTrace = new List<string>();
        }

        public FatalError(string code, string message, string stackTrace) : base(code, message)
        {
            if (!string.IsNullOrWhiteSpace(stackTrace))
                StackTrace = stackTrace.Split('\n');
            else
                StackTrace = new List<string>();
        }

        public FatalError(string code, string message, IList<string> stackTrace) : base(code, message)
        {
            StackTrace = stackTrace;
        }

        public new string LogMessage()
        {
            return $"{Code} - {Message}, Stack Trace: { StackTrace.Join()}";
        }
    }
}
