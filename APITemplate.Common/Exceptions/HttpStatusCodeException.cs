using System;
using System.Collections.Generic;

namespace APITemplate.Common.Exceptions
{
    public class HttpStatusCodeException : Exception
    {
        public int StatusCode { get; set; }
        public string ContentType { get; set; } = @"application/json";

        public List<string> Errors { get; set; }

        public HttpStatusCodeException(int statusCode)
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusCodeException(int statusCode, string message)
        {
            this.StatusCode = statusCode;
            this.Errors = new List<string> { message };
        }

        public HttpStatusCodeException(int statusCode, List<string> errors)
        {
            this.StatusCode = statusCode;
            this.Errors = errors;
        }
    }
}
