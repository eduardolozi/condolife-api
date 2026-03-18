namespace Condominiums.Application.Addresses.GetCondominiumsAddressInfos;

public record GetCondominiumAddressInfoResult(
    int CondominiumId,
    string CondominiumName,
    string StateCode,
    string City,
    string Neighborhood,
    string Street,
    string Number, 
    string PostalCode);