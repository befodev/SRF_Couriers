using CourierManagementSystem.Api.Exceptions;
using CourierManagementSystem.Api.Models.DTOs.Responses;
using System.Net;
using System.Text.Json;

namespace CourierManagementSystem.Api.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    private static readonly Dictionary<Type, (int statusCode, Func<Exception, object> body)> s_HandlerTable = new()
    {
        {
            typeof(NotFoundException),
            (
                (int)HttpStatusCode.NotFound,
                ex => new ErrorResponse
                {
                    Code = "NOT_FOUND",
                    Message = ex.Message,
                    Timestamp = DateTime.UtcNow
                }
            )
        },
        {
            typeof(ValidationException),
            (
                (int)HttpStatusCode.BadRequest,
                ex => new ValidationErrorResponse
                {
                    Errors = ((ValidationException)ex).Errors
                }
            )
        },
        {
            typeof(UnauthorizedException),
            (
                (int)HttpStatusCode.Unauthorized,
                ex => new ErrorResponse
                {
                    Code = "UNAUTHORIZED",
                    Message = ex.Message,
                    Timestamp = DateTime.UtcNow
                }
            )
        },
        {
            typeof(ForbiddenException),
            (
                (int)HttpStatusCode.Forbidden,
                ex => new ErrorResponse
                {
                    Code = "FORBIDDEN",
                    Message = ex.Message,
                    Timestamp = DateTime.UtcNow
                }
            )
        }
    };

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
            _logger.LogError(ex, "An error occurred while processing the request");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        Type exceptionType = exception.GetType();
        (int statusCode, object body) result;

        if (s_HandlerTable.TryGetValue(exceptionType, out var handler))
        {
            result = (handler.statusCode, handler.body(exception));
        }
        else
        {
            result = (
                (int)HttpStatusCode.InternalServerError,
                new ErrorResponse
                {
                    Code = "INTERNAL_SERVER_ERROR",
                    Message = "An unexpected error occurred",
                    Timestamp = DateTime.UtcNow
                }
            );
        }

        context.Response.StatusCode = result.statusCode;

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(result.body, jsonOptions));
    }
}
