using Condominiums.Application.Addresses;
using Condominiums.Application.Addresses.GetCondominiumsAddressInfos;
using Condominiums.Application.Condominiums.CreateCondominium;
using Condominiums.Application.Condominiums.GetCondominium;
using Condominiums.Application.Management.GetResidentPreRegistrationTemplate;
using Condominiums.Application.Management.ResidentPreRegistration;
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

        services.AddScoped<GetCondominiumUseCase>();

        services.AddScoped<GetResidentPreRegistrationTemplateHandler>();
        services.AddScoped<IValidator<ImportResidentPreRegistrationCommand>, ImportResidentPreRegistrationCommandValidator>();
        services.AddScoped<IValidator<ResidentPreRegistrationRow>, ResidentPreRegistrationRowValidator>();
        services.AddScoped<ImportResidentPreRegistrationUseCase>();

        services.AddScoped<GetCondominiumsAddressInfosUseCase>();
        services.AddScoped<IGetCondominiumsAddressInfosHandler, GetCondominiumsAddressInfosHandler>();
    }
}
