using Identity.Domain.Enums;

namespace Identity.Application.DTOs;

public record CondominiumMembershipDto(int CondominiumId, UserRole Role)
{
    public int CondominiumId { get; init; }
    public string CondominiumName { get; init; }
    public AddressDto Address { get; set; }
}