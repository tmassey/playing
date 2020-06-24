using System;

namespace playing.Core.Exceptions
{
    public class HttpUnauthorizedException : UnauthorizedAccessException
    {
        public HttpUnauthorizedException(string message = "Access denied") : base(message)
        {

        }
    }
}