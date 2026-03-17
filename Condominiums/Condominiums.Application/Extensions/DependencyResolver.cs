using Condominiums.Application.Commands;
using Condominiums.Application.Interfaces.Services;
using Condominiums.Application.Services;
using Condominiums.Application.UseCases.Condominiums;
using Condominiums.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Condominiums.Application.Extensions;

public static class DependencyResolver
{
    public static void AddCondominiumsApplication(this IServiceCollection services)
    {
        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<ICondominiumService, CondominiumService>();
        services.AddScoped<IValidator<CreateCondominiumCommand>, CreateCondominiumCommandValidator>();
        services.AddScoped<CreateCondominiumUseCase>();
    }
}