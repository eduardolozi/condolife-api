using Condominiums.Application.DTOs;
using Condominiums.Application.Interfaces.InfraAbstractions;
using Condominiums.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Condominiums.Application.Services;

public class AddressService(ICondominiumDbContext dbContext) : IAddressService
{
    public Task<CityStateInfoDto?> GetCityIdAndStateCodeByIbgeCode(string ibgeCode, CancellationToken ct)
    {
        return dbContext
            .Cities
            .Where(c => c.IbgeCode == ibgeCode)
            .Select(c => new CityStateInfoDto(c.Id, c.State!.Code))
            .FirstOrDefaultAsync(ct);
    }

    public Task<List<AddressDto>> GetAddressInfosByCondominiumIds(List<int> condominiumIds, CancellationToken ct)
    {
        if(condominiumIds.Count == 0)
        {
            return Task.FromResult(new List<AddressDto>());
        }

        return dbContext
            .Condominiums
            .Where(c => condominiumIds.Contains(c.Id))
            .Select(c => 
                new AddressDto(
                    c.Id,
                    string.Empty,
                    string.Empty,
                    c.Address.Neighborhood,
                    c.Address.Street,
                    c.Address.Number,
                    c.Address.PostalCode
                )
            )
            .ToListAsync(ct);
    }
}