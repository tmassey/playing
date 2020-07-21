using System;

namespace Phoenix.Api.Core.Exceptions
{
    public class HttpUnauthorizedException : UnauthorizedAccessException
    {
        public HttpUnauthorizedException(string message = "Access denied") : base(message)
        {

        }
    }
}