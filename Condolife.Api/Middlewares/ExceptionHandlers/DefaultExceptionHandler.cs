using Condolife.Api.ViewModels;
using Identity.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Condolife.Api.Middlewares.ExceptionHandlers;

public class DefaultExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = exception switch
        {
            ApplicationException => StatusCodes.Status400BadRequest,
            ConflictException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        var message = httpContext.Response.StatusCode == StatusCodes.Status500InternalServerError 
            ? "Erro inesperado"
            : exception.Message;
            
        var payload = new ResponseError(message);

        await httpContext.Response.WriteAsJsonAsync(payload, cancellationToken);
        
        return true;
    }
}