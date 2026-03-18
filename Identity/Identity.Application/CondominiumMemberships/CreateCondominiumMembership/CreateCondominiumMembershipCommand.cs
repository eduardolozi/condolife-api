using Identity.Domain.Enums;

namespace Identity.Application.CondominiumMemberships.CreateCondominiumMembership;

public class CreateCondominiumMembershipCommand
{
    public int CondominiumId { get; set; }
    public UserRole UserRole { get; set; }
}