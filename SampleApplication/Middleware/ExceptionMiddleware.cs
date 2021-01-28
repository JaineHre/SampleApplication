using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SampleApplication.ErrorHandling;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleApplication.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate requestDelegate;

        private readonly ILogger<ExceptionMiddleware> logger;

        private readonly IHostEnvironment hostEnvironment;

        public ExceptionMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionMiddleware> logger, 
            IHostEnvironment hostEnvironment)
        {
            this.requestDelegate = requestDelegate;
            this.logger = logger;
            this.hostEnvironment = hostEnvironment;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await requestDelegate(httpContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = hostEnvironment.IsDevelopment()
                    ? new ApiException(httpContext.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiException(httpContext.Response.StatusCode, "Internal Server Error");

                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await httpContext.Response.WriteAsync(json);
            }
        }
    }
}