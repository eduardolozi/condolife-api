using Condominiums.Application.Abstractions;
using Condominiums.Application.Addresses.DTOs;
using FluentValidation;
using Identity.Contracts.CondominiumMemberships.CreateCondominiumMembership;
using Microsoft.EntityFrameworkCore;

namespace Condominiums.Application.Condominiums.CreateCondominium;

public class CreateCondominiumUseCase(
    IValidator<CreateCondominiumCommand> validator, 
    ICondominiumDbContext dbContext,
    ICreateCondominiumMembershipHandler createCondominiumMembershipHandler)
{
    public async Task HandleAsync(CreateCondominiumCommand command, Guid externalUserId, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(command, ct);
        
        var cityStateInfo = await GetCityIdAndStateCodeByIbgeCode(command.IbgeCode, ct)
            ?? throw new Exception($"Não foi possível encontrar a cidade pelo código IBGE ({command.IbgeCode}).");

        var stateExists = cityStateInfo.StateCode.Equals(command.StateCode, StringComparison.InvariantCultureIgnoreCase);
        if (!stateExists) throw new Exception("A UF informada não é válida para a cidade.");

        var condominium = command.ToCondominium(cityStateInfo.CityId);
        await dbContext.Condominiums.AddAsync(condominium, ct);
        await dbContext.SaveChangesAsync(ct);

        await createCondominiumMembershipHandler.HandleAsync(condominium.Id, externalUserId, ct);
    }

    private Task<CityStateInfoDto?> GetCityIdAndStateCodeByIbgeCode(string ibgeCode, CancellationToken ct)
    {
        return dbContext
            .Cities
            .Where(c => c.IbgeCode == ibgeCode)
            .Select(c => new CityStateInfoDto(c.Id, c.State!.Code))
            .FirstOrDefaultAsync(ct);
    }
}