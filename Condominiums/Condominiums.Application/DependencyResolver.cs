using Condominiums.Application.Addresses;
using Condominiums.Application.Condominiums.CreateCondominium;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Condominiums.Application;

public static class DependencyResolver
{
    public static void AddCondominiumsApplication(this IServiceCollection services)
    {
        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<IValidator<CreateCondominiumCommand>, CreateCondominiumCommandValidator>();
        services.AddScoped<CreateCondominiumUseCase>();
    }
}