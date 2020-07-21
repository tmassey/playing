using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.EntityFrameworkCore.Internal;

namespace Phoenix.Api.Core.Errors
{

    public class Result
    {
        public bool IsSuccess => !Errors.Any();
        public List<Error> Errors { get; set; }

        public Result()
        {
            Errors = new List<Error>();
        }

        public Result(List<Error> errors)
        {
            Errors = errors;
        }

        public Result(Exception e)
        {
            Errors = new List<Error>
            {
                new FatalError(

                    HttpStatusCode.InternalServerError.ToString(),
                    e.Message,
                    e.StackTrace
                )
            };
        }

        public static Result BuildSuccess()
        {
            return new Result();
        }

        public static Result BuildError(Exception e)
        {
            return new Result(e);
        }

        public static Result BuildError(List<Error> errors)
        {
            return new Result(errors);
        }

    }

    public class Result<T> : Result
    {
        public T Data { get; set; }

        public Result() { }

        public Result(List<Error> errors) : base(errors) { }

        public Result(Exception e) : base(e) { }
        
        public Result(T data)
        {
            Data = data;
        }

        public static Result<T> BuildSuccess(T data)
        {
            return new Result<T>(data);
        }

        public new static Result<T> BuildError(Exception e)
        {
            return new Result<T>(e);
        }

        public new static Result<T> BuildError(List<Error> errors)
        {
            return new Result<T>(errors);
        }
    }
}
