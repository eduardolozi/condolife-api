using Condominiums.Contracts.Addresses.GetAddressesInfos;

namespace Condominiums.Application.Addresses.GetCondominiumsAddressInfos;

public class GetCondominiumsAddressInfosHandler(GetCondominiumsAddressInfosUseCase useCase) : IGetCondominiumsAddressInfosHandler
{
    public async Task<List<GetCondominiumAddressInfoResponse>> HandleAsync(List<int> condominiumIds, CancellationToken ct)
    {
        var result = await useCase.HandleAsync(condominiumIds, ct);
        
        return result.Select(x => 
            new GetCondominiumAddressInfoResponse
            (
                x.CondominiumId,
                x.CondominiumName,
                x.StateCode,
                x.City,
                x.Neighborhood,
                x.Street,
                x.Number,
                x.PostalCode
            )
        ).ToList();
    }
}