using Identity.Domain.Enums;

namespace Identity.Domain.Entities;

public class Invite
{
    public Guid Id { get; set; }
    public required string GuestEmail { get; set; }
    public UserRole GuestRole { get; set; }
    public DateTime InvitedAt { get; set; }
    public DateTime ExpiresAt => InvitedAt.AddMinutes(30);
    public DateTime? AcceptedAt  { get; set; }
    public string TokenHash { get; set; }
    public UserRole Role { get; set; }
    
    public int? CondominiumMembership { get; set; }

    public bool IsPending => ExpiresAt > DateTime.UtcNow && AcceptedAt == null;
}