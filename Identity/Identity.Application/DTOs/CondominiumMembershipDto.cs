using Identity.Domain.Enums;

namespace Identity.Application.DTOs;

public record CondominiumMembershipDto(int CondominiumId, UserRole Role)
{
    public string? CondominiumName { get; init; }
    public string? CondominiumShortAddress { get; init; }
}