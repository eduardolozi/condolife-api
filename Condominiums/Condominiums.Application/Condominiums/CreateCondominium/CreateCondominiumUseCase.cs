using Condominiums.Application.Abstractions;
using Condominiums.Application.Addresses;
using FluentValidation;
using Identity.Application.Interfaces.Services;

namespace Condominiums.Application.Condominiums.CreateCondominium;

public class CreateCondominiumUseCase(
    IValidator<CreateCondominiumCommand> validator, 
    IAddressService addressService,
    ICondominiumMembershipService condominiumMembershipService,
    ICondominiumDbContext dbContext)
{
    public async Task HandleAsync(CreateCondominiumCommand command, Guid externalUserId, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(command, ct);
        var cityStateInfo = await addressService.GetCityIdAndStateCodeByIbgeCode(command.IbgeCode, ct)
            ?? throw new Exception($"Não foi possível encontrar a cidade pelo código IBGE ({command.IbgeCode}).");

        var stateExists = cityStateInfo.StateCode.Equals(command.StateCode, StringComparison.InvariantCultureIgnoreCase);
        if (!stateExists) throw new Exception("A UF informada não é válida para a cidade.");

        var condominium = command.ToCondominium(cityStateInfo.CityId);
        await dbContext.Condominiums.AddAsync(condominium, ct);
        await dbContext.SaveChangesAsync(ct);

        await condominiumMembershipService.CreateSyndicMembership(condominium.Id, externalUserId, ct);
    }
}