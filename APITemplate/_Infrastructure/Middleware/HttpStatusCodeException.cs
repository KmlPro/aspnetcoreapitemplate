using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITemplate._Infrastructure.Middleware
{
    public class HttpStatusCodeException : Exception
    {
        public int StatusCode { get; set; }
        public string ContentType { get; set; } = @"application/json";

        public List<string> Errors { get; set; }

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
