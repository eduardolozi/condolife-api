using Condominiums.Application.Addresses;
using Condominiums.Application.Addresses.GetCondominiumsAddressInfos;
using Condominiums.Application.Condominiums.CreateCondominium;
using Condominiums.Contracts.Addresses.GetAddressesInfos;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Condominiums.Application;

public static class DependencyResolver
{
    public static void AddCondominiumsApplication(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateCondominiumCommand>, CreateCondominiumCommandValidator>();
        services.AddScoped<CreateCondominiumUseCase>();

        services.AddScoped<GetCondominiumsAddressInfosUseCase>();
        services.AddScoped<IGetCondominiumsAddressInfosHandler, GetCondominiumsAddressInfosHandler>();
    }
}