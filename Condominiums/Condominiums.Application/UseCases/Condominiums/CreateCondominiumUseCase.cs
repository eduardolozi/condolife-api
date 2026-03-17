using Condominiums.Application.Commands;
using Condominiums.Application.Interfaces.Services;
using FluentValidation;
using Identity.Application.Interfaces.Services;

namespace Condominiums.Application.UseCases.Condominiums;

public class CreateCondominiumUseCase(
    IValidator<CreateCondominiumCommand> validator, 
    IAddressService addressService,
    ICondominiumService condominiumService,
    ICondominiumMembershipService condominiumMembershipService)
{
    public async Task HandleAsync(CreateCondominiumCommand command, Guid externalUserId, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(command, ct);
        var cityStateInfo = await addressService.GetCityIdAndStateCodeByIbgeCode(command.IbgeCode, ct)
            ?? throw new Exception($"Não foi possível encontrar a cidade pelo código IBGE ({command.IbgeCode}).");

        var stateExists = cityStateInfo.StateCode.Equals(command.StateCode, StringComparison.InvariantCultureIgnoreCase);
        if (!stateExists) throw new Exception("A UF informada não é válida para a cidade.");

        var condominium = command.ToCondominium(cityStateInfo.CityId);
        await condominiumService.Create(condominium, ct);

        await condominiumMembershipService.CreateSyndicMembership(condominium.Id, externalUserId, ct);
    }
}