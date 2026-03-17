using Condominiums.Application.Addresses.DTOs;

namespace Condominiums.Application.Addresses;

public interface IAddressService
{
    Task<CityStateInfoDto?> GetCityIdAndStateCodeByIbgeCode(string ibgeCode, CancellationToken ct);
    Task<List<AddressDto>> GetAddressInfosByCondominiumIds(List<int> condominiumIds, CancellationToken ct);
}