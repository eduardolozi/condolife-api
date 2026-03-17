using Identity.Application.DTOs;
using Identity.Application.Interfaces.Services;

namespace Identity.Application.UseCases.CondominiumMemberships;

public class GetCondominiumMembershipsUseCase(
    ICondominiumMembershipService condominiumMembershipService) 
{
    public async Task<List<CondominiumMembershipDto>> HandleAsync(Guid externalUserId, CancellationToken ct)
    {
        var membershipsInfo = await condominiumMembershipService.GetInfos(externalUserId, ct);
        // var addresses = await addressService.
        
        return membershipsInfo;
    }
}