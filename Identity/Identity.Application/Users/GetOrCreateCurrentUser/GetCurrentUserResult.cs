using Identity.Application.CondominiumMemberships.GetCondominiumMemberships;

namespace Identity.Application.Users.GetOrCreateCurrentUser;

public record GetCurrentUserResult(int UserId, Guid ExternalUserId, string FullName, string Email)
{
    public string? AvatarUrl { get; init; }
    public List<GetCondominiumMembershipsResult> Condominiums { get; init; } = [];
}