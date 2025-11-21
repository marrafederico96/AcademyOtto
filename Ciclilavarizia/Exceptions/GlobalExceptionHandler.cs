using AuthLibrary.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Ciclilavarizia.Exceptions
{
    public class GlobalExceptionHandler(IHostEnvironment env) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails();

            switch (exception)
            {
                case NotFoundException:
                    problemDetails.Title = "Not Found";
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Detail = exception.Message;
                    Log.Error($"Item not found: {exception.Message}");
                    break;
                case UnauthorizedAccessException:
                    problemDetails.Title = "Unauthorized Access";
                    problemDetails.Status = StatusCodes.Status401Unauthorized;
                    problemDetails.Detail = exception.Message;
                    Log.Error($"Login failed: {exception.Message}");
                    break;
                case UserAlreadyExistsException:
                    problemDetails.Title = "User already exists";
                    problemDetails.Status = StatusCodes.Status409Conflict;
                    problemDetails.Detail = exception.Message;
                    Log.Error($"Registration failed: {exception.Message}");
                    break;
                case PasswordMismatchException:
                    problemDetails.Title = "Passwords Mismatch";
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Detail = exception.Message;
                    break;
                case PasswordExpiredException:
                    problemDetails.Title = "Password Expired";
                    problemDetails.Status = StatusCodes.Status403Forbidden;
                    problemDetails.Detail = exception.Message;
                    break;
                default:
                    problemDetails.Detail = env.IsDevelopment() ? exception.Message : "Errore interno. Riprovate";
                    problemDetails.Title = "Internal Server Error";
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    Log.Error(exception, "Unhandled server error");
                    break;

            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
