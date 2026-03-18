using Identity.Application.Common.DTOs;
using Identity.Domain.Enums;

namespace Identity.Application.CondominiumMemberships.GetCondominiumMemberships;

public record GetCondominiumMembershipsResult(int CondominiumId, UserRole Role)
{
    public AddressDto Address { get; set; }
}