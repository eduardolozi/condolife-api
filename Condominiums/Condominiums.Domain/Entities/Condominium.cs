using System.ComponentModel.DataAnnotations.Schema;

namespace Condominiums.Domain.Entities;

public class Condominium
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required Address Address { get; set; }

    public List<Tower> Towers { get; set; } = [];
    public List<ResidentProspect> ResidentProspects { get; set; } = [];
}
