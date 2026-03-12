namespace Condominiums.Domain.Entities;

public class Tower
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int ApartmentsPerFloor  { get; set; }
    public int FloorCount  { get; set; }
    
    public int CondominiumId  { get; set; }
    public Condominium? Condominium { get; set; }
}