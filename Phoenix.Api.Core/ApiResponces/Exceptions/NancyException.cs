using System;
using System.Net;

namespace Phoenix.Api.Core.ApiResponces.Exceptions
{
    public class NancyHttpException : Exception
    {
        public HttpStatusCode HttpStatus { get; set; }
        public string Description { get; set; }

        public NancyHttpException(HttpStatusCode httpStatus, string message) : base(message)
        {
            HttpStatus = httpStatus;
        }
    }
}
