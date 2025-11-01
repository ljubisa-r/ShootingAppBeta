using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

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
            _logger.LogError(ex, "Unhandled exception occurred while processing request.");

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        // Default to internal server error
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var message = "Something went wrong while processing your request.";

        // You can customize for known exception types
        switch (exception)
        {
            case ArgumentException:
            case FormatException:
                statusCode = (int)HttpStatusCode.BadRequest;
                message = "Invalid request data.";
                break;
            case KeyNotFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                message = "Requested resource not found.";
                break;
            case InvalidOperationException:
                statusCode = (int)HttpStatusCode.NotFound;
                message = "Requested resource not found.";
                break;
            case DbUpdateException:
                statusCode = (int)HttpStatusCode.InternalServerError;
                message = "Database error occurred.";
                break;
            case TimeoutException:
                statusCode = (int)HttpStatusCode.RequestTimeout;
                message = "The request timed out.";
                break;
            default:
                statusCode = (int)HttpStatusCode.InternalServerError;
                message = "An unexpected error occurred.";
                break;

        }

        var response = new
        {
            error = message,
            traceId = context.TraceIdentifier // helpful for debugging/log correlation
        };

        context.Response.StatusCode = statusCode;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}
