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

        var validationErrors = validationException.Errors
            .Select(x => new ResponseValidationError(
                string.IsNullOrWhiteSpace(x.PropertyName) ? null : x.PropertyName,
                x.ErrorMessage,
                x.CustomState as int?))
            .ToList();

        var payload = new ResponseError("Um ou mais erros de validação ocorreram", validationErrors);

        await httpContext.Response.WriteAsJsonAsync(payload, cancellationToken);

        return true;
    }
}
