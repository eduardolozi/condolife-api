using Condominiums.Domain.Entities;

namespace Condominiums.Application.Condominiums.CreateCondominium;

public class CreateCondominiumCommand
{
    public string CondominiumName { get; set; }
    public string PostalCode {get; set;}
    public string Neighborhood {get; set;}
    public string Street {get; set;}
    public string City {get; set;}
    public string StateCode {get; set;}
    public string Number  {get; set;}
    public string IbgeCode {get; set;}
    public string? Complement { get; set; }

    public Condominium ToCondominium(int cityId) => new Condominium
    {
        Name = CondominiumName,
        Address = new Address
        {
            CityId = cityId,
            Neighborhood = Neighborhood,
            Number = Number,
            PostalCode = PostalCode,
            Street = Street,
            Complement = Complement

        }
    };
}