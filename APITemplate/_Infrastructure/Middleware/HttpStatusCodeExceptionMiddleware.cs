using APITemplate.Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APITemplate._Infrastructure.Middleware
{
    public class HttpStatusCodeExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpStatusCodeExceptionMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HttpStatusCodeException ex)
            {
                if (context.Response.HasStarted)
                {
                    throw;
                }
                WriteInternalLog(ex);
                await WriteResponse(context, ex.StatusCode, ex.ContentType, ex.Errors);

                return;
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    throw;
                }
                WriteInternalLog(ex);
                await WriteResponse(context, 500, @"application/json", new List<string>() { "An unexpected error while handling the request" });

                return;
            }
        }

        private async Task WriteResponse(HttpContext context, int statusCode, string contentType, List<string> errors)
        {
            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = contentType;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(new { errors }));
        }

        private void WriteInternalLog(Exception ex)
        {
            Log.Fatal($"An unexpected error while handling the request: '{ex}'");
        }
    }

    public static class HttpStatusCodeExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpStatusCodeExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpStatusCodeExceptionMiddleware>();
        }
    }
}
