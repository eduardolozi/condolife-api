using Condominiums.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Condominiums.Application.Addresses.GetCondominiumsAddressInfos;

public class GetCondominiumsAddressInfosUseCase(ICondominiumDbContext dbContext)
{
    public Task<List<GetCondominiumAddressInfoResult>> HandleAsync(List<int> condominiumIds, CancellationToken ct)
    {
        if (condominiumIds.Count == 0)
        {
            return Task.FromResult(new List<GetCondominiumAddressInfoResult>());
        }

        return dbContext
            .Condominiums
            .Where(c => condominiumIds.Contains(c.Id))
            .Join(
                dbContext.Cities,
                c => c.Address.CityId,
                city => city.Id,
                (condominium, city) => new {condominium, city}
            )
            .Select(c => 
                new GetCondominiumAddressInfoResult(
                    c.condominium.Id,
                    c.condominium.Name,
                    c.city.State!.Code,
                    c.city.Name,
                    c.condominium.Address.Neighborhood,
                    c.condominium.Address.Street,
                    c.condominium.Address.Number,
                    c.condominium.Address.PostalCode
                )
            )
            .ToListAsync(ct);
    }
}