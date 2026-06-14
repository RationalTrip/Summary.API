using Summary.API.Models;
using Summary.Core.Constants;
using Summary.Core.Exceptions;

namespace Summary.API.Middlewares;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;

    public CustomExceptionHandlerMiddleware(RequestDelegate next, IWebHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext httpContext, ILogger<CustomExceptionHandlerMiddleware> logger)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            HandleException(httpContext, ex, logger);
        }
    }

    private void HandleException(HttpContext context, Exception ex, ILogger<CustomExceptionHandlerMiddleware> logger)
    {
        GeneralErrorResponse errorResponse;
        int statusCode;

        switch (ex)
        {
            case SummaryBaseException customException:
                errorResponse = new GeneralErrorResponse
                {
                    ErrorCode = customException.ErrorCode,
                    Message = ex.Message,
                    Details = customException.Details
                };

                statusCode = customException.StatusCode;
                break;

            case Exception debugException when _env.IsDevelopment():
                errorResponse = new GeneralErrorResponse
                {
                    ErrorCode = ExceptionConstants.UnexpectedErrorCode,
                    Message = ex.Message,
                    Details = debugException.StackTrace
                };

                statusCode = ExceptionConstants.BadRequestStatusCode;
                break;
            default:
                errorResponse = new GeneralErrorResponse
                {
                    ErrorCode = ExceptionConstants.UnexpectedErrorCode,
                    Message = "Unexpected error occured. Please contact administrators.",
                };

                statusCode = ExceptionConstants.BadRequestStatusCode;
                break;
        }

        logger.LogError(ex, "An exception occurred with error code {ErrorCode} and message {Message}", errorResponse.ErrorCode, ex.Message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        context.Response.WriteAsJsonAsync(errorResponse);
    }
}
