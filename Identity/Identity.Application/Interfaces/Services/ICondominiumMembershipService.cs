using Identity.Application.DTOs;

namespace Identity.Application.Interfaces.Services;

public interface ICondominiumMembershipService
{
    Task CreateSyndicMembership(int condominiumId, Guid externalUserId, CancellationToken ct);
    Task<List<CondominiumMembershipDto>> GetInfos(Guid externalUserId, CancellationToken ct);
}