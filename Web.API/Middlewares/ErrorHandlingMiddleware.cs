using RelatoX.Application.DTOs;
using System.Net;
using System.Text.Json;

namespace RelatoX.Infra.Middlewares;

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

        var statusCode = HttpStatusCode.InternalServerError;
        var message = "An unexpected error occurred. Please try again later or contact support if the problem persists";

        switch (exception)
        {
            case KeyNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
                _logger.LogError(exception, message);
                break;

            case ArgumentException:
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
                _logger.LogError(exception, message);
                break;

            default:
                _logger.LogError(exception, message);
                break;
        }

        response.StatusCode = (int)statusCode;

        var apiResponse = ApiResponse<string>.Fail(message);
        var json = JsonSerializer.Serialize(apiResponse);

        await response.WriteAsync(json);
    }
}