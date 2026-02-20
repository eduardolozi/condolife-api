namespace Identity.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public Guid ExternalId { get; set; }
    
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public string? AvatarUrl { get; set; }

    public List<CondominiumMembership> Condominiums { get; set; } = [];
}