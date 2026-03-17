using Identity.Application.DTOs;
using Identity.Application.Interfaces.InfraAbstractions;
using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Services;

public class CondominiumMembershipService(IIdentityDbContext dbContext) : ICondominiumMembershipService
{
    public async Task CreateSyndicMembership(int condominiumId, Guid externalUserId, CancellationToken ct)
    {
        var userId = await GetUserId(externalUserId, ct);

        var membership = new CondominiumMembership
        {
            CondominiumId = condominiumId,
            JoinDate = DateTime.UtcNow,
            UserId = userId,
            UserRole = UserRole.Syndic
        };
        
        await dbContext.CondominiumMemberships.AddAsync(membership, ct);
        await dbContext.SaveChangesAsync(ct);
    }
    
    public async Task<List<CondominiumMembershipDto>> GetInfos(Guid externalUserId, CancellationToken ct)
    {
        var userId = await GetUserId(externalUserId, ct);

        return await dbContext
            .CondominiumMemberships
            .Where(cm => cm.UserId == userId)
            .Select(cm => new CondominiumMembershipDto(cm.CondominiumId, cm.UserRole))
            .ToListAsync(ct);
    }

    private async Task<int> GetUserId(Guid externalUserId, CancellationToken ct)
    {
        var userId = await dbContext
            .Users
            .Where(x => x.ExternalId == externalUserId)
            .Select(u => u.Id)
            .FirstOrDefaultAsync(ct);
        
        return userId == 0 ? throw new Exception("O usuário solicitante não foi encontrado.") : userId;
    }
}