using Identity.Application.DTOs;

namespace Identity.Application.Responses;

public record GetCurrentUserResponse(int UserId, Guid ExternalUserId, string FullName, string Email)
{
    public string? AvatarUrl { get; init; }
    public List<CondominiumMembershipDto> Condominiums { get; init; } = [];
}