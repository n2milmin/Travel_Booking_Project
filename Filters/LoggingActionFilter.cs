using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.NetworkInformation;

namespace GBC_Travel_Group_136.Filters
{
    public class LoggingActionFilter : ActionFilterAttribute
    {
        private readonly ILogger<LoggingActionFilter> _logger;

        public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Log the action being executed
            _logger.LogInformation($"Action {context.ActionDescriptor.DisplayName} is executing.");

            // If needed, you can access parameters or other context details
            // var parameters = context.ActionArguments;

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // Log the action that was executed
            _logger.LogInformation($"Action {context.ActionDescriptor.DisplayName} executed.");

            // You can access the result here if needed
            // var result = context.Result;

            base.OnActionExecuted(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            // Log the result about to be executed
            _logger.LogInformation($"Result {context.Result} is executing.");

            base.OnResultExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            // Log the result that was executed
            _logger.LogInformation($"Result {context.Result} executed.");

            base.OnResultExecuted(context);
        }
    }

}
