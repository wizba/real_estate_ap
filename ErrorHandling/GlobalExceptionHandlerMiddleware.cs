using System.Diagnostics;

namespace real_estate_api.ErrorHandling
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                TraceId = Activity.Current?.Id ?? context.TraceIdentifier
            };

            switch (exception)
            {
                case NotFoundException notFoundEx:
                    response.StatusCode = StatusCodes.Status404NotFound;
                    errorResponse.Type = "NotFound";
                    errorResponse.Message = $"{notFoundEx.Message} hello middleware is working";
                    break;

                case BadRequestException badRequestEx:
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    errorResponse.Type = "BadRequest";
                    errorResponse.Message = badRequestEx.Message;
                    break;

                case ValidationException validationEx:
                    response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                    errorResponse.Type = "ValidationError";
                    errorResponse.Message = "One or more validation errors occurred.";
                    errorResponse.Errors = validationEx.Errors;
                    break;

                default:
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    errorResponse.Type = "ServerError";
                    errorResponse.Message = "An unexpected error occurred.";
                    _logger.LogError(exception, "An unexpected error occurred.");
                    break;
            }

            await response.WriteAsJsonAsync(errorResponse);
        }
    }
}
