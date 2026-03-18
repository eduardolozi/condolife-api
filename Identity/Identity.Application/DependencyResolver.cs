using FluentValidation;
using Identity.Application.CondominiumMemberships.CreateCondominiumMembership;
using Identity.Application.CondominiumMemberships.GetCondominiumMemberships;
using Identity.Application.Users.GetOrCreateCurrentUser;
using Identity.Contracts.CondominiumMemberships.CreateCondominiumMembership;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Application;

public static class DependencyResolver
{
    public static void AddIdentityApplication(this IServiceCollection services)
    {
        services.AddScoped<IValidator<GetOrCreateCurrentUserCommand>, CreateCurrentUserCommandValidator>();
        services.AddScoped<GetOrCreateCurrentUserUseCase>();
        
        services.AddScoped<GetCondominiumMembershipsUseCase>();
        
        services.AddScoped<CreateCondominiumMembershipUseCase>();
        services.AddScoped<ICreateCondominiumMembershipHandler, CreateCondominiumMembershipHandler>();
    }
}