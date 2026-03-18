using Condominiums.Contracts.Addresses.GetAddressesInfos;
using Identity.Application.Abstractions;
using Identity.Application.Common.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.CondominiumMemberships.GetCondominiumMemberships;

public class GetCondominiumMembershipsUseCase(IIdentityDbContext dbContext, IGetCondominiumsAddressInfosHandler getCondominiumsAddressInfosHandler) 
{
    public async Task<List<GetCondominiumMembershipsResult>> HandleAsync(Guid externalUserId, CancellationToken ct)
    {
        var userId = await dbContext
            .Users
            .Where(x => x.ExternalId == externalUserId)
            .Select(u => u.Id)
            .FirstOrDefaultAsync(ct);

        if (userId == 0) throw new Exception("O usuário solicitante não foi encontrado.");
        
        var membershipsInfo = await GetMemberships(userId, ct);
        var condominiumsIds = membershipsInfo.Select(x => x.CondominiumId).ToList();
        var condominiumAddresses = await getCondominiumsAddressInfosHandler.HandleAsync(condominiumsIds, ct);
        
        MapAddresses(membershipsInfo, condominiumAddresses);
        
        return membershipsInfo;
    }

    private Task<List<GetCondominiumMembershipsResult>> GetMemberships(int userId, CancellationToken ct)
    {
        return dbContext
            .CondominiumMemberships
            .Where(cm => cm.UserId == userId)
            .Select(cm => new GetCondominiumMembershipsResult(cm.CondominiumId, cm.UserRole))
            .ToListAsync(ct);
    }

    private static void MapAddresses(List<GetCondominiumMembershipsResult> memberships, List<GetCondominiumAddressInfoResponse> addresses)
    {
        var addressesByCondominiumId = addresses.ToDictionary(x => x.CondominiumId);

        foreach (var membership in memberships)
        {
            if (addressesByCondominiumId.TryGetValue(membership.CondominiumId, out var addr))
                membership.Address = new AddressDto(addr);
        }
    }
    
}