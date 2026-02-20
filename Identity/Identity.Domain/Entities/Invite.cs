using Identity.Domain.Enums;

namespace Identity.Domain.Entities;

public class Invite
{
    public Guid Id { get; set; }
    public required string GuestEmail { get; set; }
    public UserRole GuestRole { get; set; }
    public DateTime InvitedAt { get; set; }
    public DateTime ExpiresAt => InvitedAt.AddDays(7);
    public DateTime? AcceptedAt  { get; set; }
    
    public int? UserId { get; set; }
    public int CondominiumId { get; set; }
}