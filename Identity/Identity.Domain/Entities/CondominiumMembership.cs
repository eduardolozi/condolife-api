using Identity.Domain.Enums;

namespace Identity.Domain.Entities;

public class CondominiumMembership
{
    public int Id { get; set; }
    
    public UserRole Role { get; set; }
    public DateTime JoinDate { get; set; }
    
    public int CondominiumId { get; set; }
    public int UserId { get; set; }
}