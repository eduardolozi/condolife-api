namespace Condominiums.Contracts.Addresses.GetAddressesInfos;

public record GetCondominiumAddressInfoResponse(
    int CondominiumId,
    string CondominiumName,
    string StateCode,
    string City,
    string Neighborhood,
    string Street,
    string Number, 
    string PostalCode);