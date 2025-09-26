namespace Application.Contracts.Customer;

public record CustomerResponse(
    string Id,
    string Name,
    string Email,
    string PhoneNumber
);
