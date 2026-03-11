using FluentValidation;
using Identity.Application.Commands;
using Identity.Application.Interfaces.Services;
using Identity.Application.Services;
using Identity.Application.UseCases.Users;
using Identity.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application.Extensions;

public static class DependencyResolver
{
    public static void AddIdentityApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IInviteService, InviteService>();
        
        services.AddScoped<IValidator<GetOrCreateCurrentUserCommand>, CreateCurrentUserCommandValidator>();
        services.AddScoped<GetOrCreateCurrentUserUseCase>();
    }
}