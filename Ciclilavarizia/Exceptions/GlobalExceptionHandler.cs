using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

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
                    break;
                case UnauthorizedAccessException:
                    problemDetails.Title = " Unauthorized Access";
                    problemDetails.Status = StatusCodes.Status401Unauthorized;
                    problemDetails.Detail = exception.Message;
                    break;
                default:
                    problemDetails.Detail = env.IsDevelopment() ? exception.Message : "Errore interno. Riprovate";
                    problemDetails.Title = "Internal Server Error";
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    break;

            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
