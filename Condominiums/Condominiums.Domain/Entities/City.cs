namespace Condominiums.Domain.Entities;

public class City
{
    public int Id { get; set; }
    public required string Name { get; set; }
    
    public int StateId { get; set; }
    public State? State { get; set; }
}