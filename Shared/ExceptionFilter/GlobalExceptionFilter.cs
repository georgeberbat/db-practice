using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Exceptions;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Shared.ExceptionFilter
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;
        private readonly IOptions<JsonOptions> _options;

        private const string ErrorPath = "errors";

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IOptions<JsonOptions> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void OnException(ExceptionContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var exception = context.Exception;

            // т.к. типы могут быть Generic
            var type = exception.GetType().Name.Replace("`1", "", StringComparison.InvariantCulture);
            if (exception.Data["Type"] != null)
            {
                type = $"{type}.{exception.Data["Type"]}";
            }

            var response = new ErrorResponse
            {
                Message = exception.Message,
                Type = $"{ErrorPath}.{type}",
                Data = exception.Data
            };

            var result = new ContentResult
            {
                StatusCode = ResolveHttpStatusCode(exception),
                ContentType = "application/json"
            };

            if (result.StatusCode == 500 || exception is ArgumentException)
            {
                var isStackTrace = exception is not (NotImplementedException or NotSupportedException);
                response.StackTrace = isStackTrace ? exception.StackTrace : null;
                _logger.LogError(exception, "Request failed");
            }
            else
            {
                _logger.LogWarning("Request complete with warning. {@Detail}", response);
            }

            result.Content = JsonSerializer.Serialize(response, _options.Value.JsonSerializerOptions);
            context.Result = result;
        }

        private static int ResolveHttpStatusCode(Exception exception)
        {
            return exception switch
            {
                UnauthorizedAccessException _ => Status401Unauthorized,

                EntityExistException _ => Status409Conflict,

                FileNotFoundException => Status404NotFound,
                EntityNotFoundException _ => Status404NotFound,

                ArgumentException _ => Status400BadRequest,

                ValidationException _ => Status400BadRequest,
                FormatException _ => Status400BadRequest,

                TimeoutException _ => Status408RequestTimeout,

                _ => 500
            };
        }
    }
}