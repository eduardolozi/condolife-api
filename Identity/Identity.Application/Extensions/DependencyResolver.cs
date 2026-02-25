using FluentValidation;
using Identity.Application.Commands;
using Identity.Application.UseCases;
using Identity.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application.Extensions;

public static class DependencyResolver
{
    public static void AddIdentityApplication(this IServiceCollection services)
    {
        services.AddScoped<IValidator<GetOrCreateCurrentUserCommand>, GetOrCreateCurrentUserCommandValidator>();
        services.AddScoped<GetOrCreateCurrentUserUseCase>();
    }
}