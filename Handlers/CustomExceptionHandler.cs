using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace AspNetCoreProblemDetailsDemo.Handlers
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
            Exception exception, CancellationToken cancellationToken)
        {
            var statusCode = exception switch
            {
                BadHttpRequestException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            var problemDetails = new ProblemDetails
            {
                Title = statusCode == StatusCodes.Status500InternalServerError
                    ? "Internal Server Error"
                    : "A handled exception occurred",
                Status = statusCode,
                Type = exception?.GetType().Name,
                Detail = exception?.Message,
                Instance = httpContext.Request.Path
            };

            //problemDetails.Extensions.Add("requestId", httpContext.TraceIdentifier);
            
            //Activity? activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;
            //problemDetails.Extensions.TryAdd("traceId", activity?.Id);

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}