using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GBC_Travel_Group_136.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log the incoming request details
            _logger.LogInformation($"Incoming Request: {context.Request.Path}");

            // Call the next delegate/middleware in the pipeline
            await _next(context);

            // Log the response status code
            _logger.LogInformation($"Response Status Code: {context.Response.StatusCode}");
        }
    }

}
