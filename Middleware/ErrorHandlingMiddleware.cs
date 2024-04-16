using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GBC_Travel_Group_136.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Call the next delegate/middleware in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex}");

                // Customize error handling based on exception type or status code
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                // You can provide custom error pages or responses here
                await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
            }
        }
    }

}
