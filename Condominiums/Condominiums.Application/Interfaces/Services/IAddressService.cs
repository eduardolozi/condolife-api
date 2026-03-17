using Condominiums.Application.DTOs;

namespace Condominiums.Application.Interfaces.Services;

public interface IAddressService
{
    Task<CityStateInfoDto?> GetCityIdAndStateCodeByIbgeCode(string ibgeCode, CancellationToken ct);
    Task<List<AddressDto>> GetAddressInfosByCondominiumIds(List<int> condominiumIds, CancellationToken ct);
}