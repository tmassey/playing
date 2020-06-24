using System;
using System.Net;

namespace playing.Core.Exceptions
{
    public class HttpException : Exception
    {
        public HttpStatusCode HttpStatus { get; set; }
        public string Description { get; set; }

        public HttpException(HttpStatusCode httpStatus, string message) : base(message)
        {
            HttpStatus = httpStatus;
        }
    }
}