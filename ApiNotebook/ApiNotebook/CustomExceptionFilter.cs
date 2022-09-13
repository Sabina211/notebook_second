using System;
using ApiNotebook.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ApiNotebook
{
    public class CustomExceptionFilter : Attribute, IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var actionName = context.ActionDescriptor.DisplayName;
            var exceptionStack = context.Exception.StackTrace;
            var exceptionMessage = context.Exception.Message;
            if (context.Exception is ApiAuthenticationException)
            {
                
            }
            context.Result = new JsonResult(
                new {error = $"{exceptionMessage} \n {exceptionStack}"}) 
                {
                    StatusCode = 400
                };
            _logger.LogError($"В методе {actionName} возникло исключение: \n {exceptionMessage} \n {exceptionStack}\n");
        }
    }
}
