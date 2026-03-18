namespace Condominiums.Contracts.Addresses.GetAddressesInfos;

public interface IGetCondominiumsAddressInfosHandler
{
    Task<List<GetCondominiumAddressInfoResponse>> HandleAsync(List<int> condominiumIds, CancellationToken ct);
}