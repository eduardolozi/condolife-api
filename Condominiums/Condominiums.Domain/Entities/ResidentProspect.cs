namespace Condominiums.Domain.Entities;

public class ResidentProspect
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Cpf { get; set; }
    public required string Apartment { get; set; }
    public string? Block { get; set; }

    public int CondominiumId { get; set; }
    public Condominium? Condominium { get; set; }
}
