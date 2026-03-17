namespace Condominiums.Application.Addresses.DTOs;

public record AddressDto(
    int CondominiumId,
    string StateCode,
    string City,
    string Neighborhood,
    string Street,
    string Number, 
    string PostalCode);