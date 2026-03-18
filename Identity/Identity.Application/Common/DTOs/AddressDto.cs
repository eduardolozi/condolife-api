using Condominiums.Contracts.Addresses.GetAddressesInfos;

namespace Identity.Application.Common.DTOs;

public record AddressDto
{
    public string CondominiumName { get; init; }
    public string StateCode { get; init; }
    public string City { get; init; }
    public string Neighborhood { get; init; }
    public string Street { get; init; }
    public string Number { get; init; }
    public string PostalCode {get; init;}

    public AddressDto(
        string condominiumName,
        string stateCode,
        string city,
        string neighborhood,
        string street,
        string number,
        string postalCode)
    {
        CondominiumName = condominiumName;
        StateCode = stateCode;
        City = city;
        Neighborhood = neighborhood;
        Street = street;
        Number = number;
        PostalCode = postalCode;
    }
    
    public AddressDto(GetCondominiumAddressInfoResponse addressInfo)
    {
        CondominiumName = addressInfo.CondominiumName;
        StateCode = addressInfo.StateCode;
        City = addressInfo.City;
        Neighborhood = addressInfo.Neighborhood;
        Street = addressInfo.Street;
        Number = addressInfo.Number;
        PostalCode = addressInfo.PostalCode;
    }
}
