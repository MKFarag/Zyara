namespace Application.Contracts.Customer.Address;

public record AddressResponse(
    string Id,
    string Governorate,
    string City,
    string Street,
    string? Note,
    bool IsDefault
);
