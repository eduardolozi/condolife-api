using System.ComponentModel.DataAnnotations.Schema;

namespace Condominiums.Domain.Entities;

[ComplexType]
public class Address
{
    public required string PostalCode { get; set; }
    public required string Neighborhood { get; set; }
    public required string Street { get; set; }
    public required string Number { get; set; }
    public string? Complement { get; set; }
    
    public int CityId { get; set; }
}