using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace MedoraClinic.API.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (httpContext.Response.HasStarted)
            {
                return false;
            }


            var (status, title, detail) = exception switch
            {
                KeyNotFoundException or NotFoundException =>
                    (StatusCodes.Status404NotFound, "Resource Not Found", exception.Message ?? "The requested resource could not be found."),

                ArgumentException or BadRequestException =>
                    (StatusCodes.Status400BadRequest, "Bad Request", exception.Message ?? "The request could not be processed."),

                AuthenticationException =>
                    (StatusCodes.Status401Unauthorized, "Authentication Failed", exception.Message ?? "Authentication Failed"),

                UnauthorizedAccessException =>
                    (StatusCodes.Status401Unauthorized, "Unauthorized", "Authentication is required to access this resource."),

                InvalidOperationException or ConflictException =>
                    (StatusCodes.Status409Conflict, "Conflict", exception.Message ?? "The request conflicts with the current resource state."),

                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error",
                        "An unexpected error occurred.")
            };



            var problemDetails = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(
                new ExceptionApiResponse(false, problemDetails));

            return true;
        }
        private record ExceptionApiResponse(bool Success, ProblemDetails Error);

    }
    public class BadRequestException(string message) : Exception(message);
    public class ConflictException(string message) : Exception(message);
    public class NotFoundException(string message) : Exception(message);
    public class AuthenticationException(string message) : Exception(message);


}
