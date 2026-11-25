using MedoraClinic.Application.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace MedoraClinic.API.Exceptions;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,Exception exception,CancellationToken cancellationToken)
    {
        if (httpContext.Response.HasStarted)
            return false;

        var (statusCode, message) = exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
            BadRequestException => (StatusCodes.Status400BadRequest, exception.Message),
            ConflictException => (StatusCodes.Status409Conflict, exception.Message),
            UnauthorizedException => (StatusCodes.Status401Unauthorized, exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "حدث خطأ غير متوقع.")
        };

        if (statusCode >= 500)
            _logger.LogError(exception, "Unhandled exception for {Path}", httpContext.Request.Path);
        else
            _logger.LogInformation(exception, "Request failed for {Path}", httpContext.Request.Path);

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json; charset=utf-8";

        var response = ApiResponse<object>.Failure(
            message,
            statusCode: statusCode);

        await httpContext.Response.WriteAsJsonAsync(
            response,
            cancellationToken);

        return true;
    }
}

public sealed class BadRequestException(string message) : Exception(message)
{
}

public sealed class ConflictException(string message) : Exception(message)
{
}

public sealed class NotFoundException(string message) : Exception(message)
{
}

public sealed class UnauthorizedException(string message) : Exception(message)
{
}
