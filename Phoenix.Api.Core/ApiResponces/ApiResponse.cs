using System.Collections.Generic;
using Phoenix.Api.Core.Errors;

namespace Phoenix.Api.Core.ApiResponces
{
    public class ApiResponse
    {
        public List<Error> Errors { get; set; }

        public ApiResponse()
        {
            Errors = new List<Error>();
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }
    }

    public class ApiFatalResponse : ApiResponse
    {
        public new List<FatalError> Errors { get; set; }

        public ApiFatalResponse()
        {
            Errors = new List<FatalError>();
        }
    }
}