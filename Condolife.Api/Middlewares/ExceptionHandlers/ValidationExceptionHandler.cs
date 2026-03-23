using Condolife.Api.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Condolife.Api.Middlewares.ExceptionHandlers;

public class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException) return false;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var errors = validationException.Errors
            .Select(x => x.ErrorMessage)
            .ToList();

        var payload = new ResponseError("Um ou mais erros de validação ocorreram", errors);
        
        await httpContext.Response.WriteAsJsonAsync(payload, cancellationToken);
        
        return true;
    }
}