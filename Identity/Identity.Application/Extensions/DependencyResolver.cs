using FluentValidation;
using Identity.Application.Commands;
using Identity.Application.Interfaces.Services;
using Identity.Application.Services;
using Identity.Application.UseCases.CondominiumMemberships;
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
        services.AddScoped<ICondominiumMembershipService, CondominiumMembershipService>();
        
        services.AddScoped<IValidator<GetOrCreateCurrentUserCommand>, CreateCurrentUserCommandValidator>();
        services.AddScoped<GetOrCreateCurrentUserUseCase>();
        services.AddScoped<GetCondominiumMembershipsUseCase>();
    }
}