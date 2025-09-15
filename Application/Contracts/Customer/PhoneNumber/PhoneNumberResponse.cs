namespace Application.Contracts.Customer.PhoneNumber;

public record PhoneNumberResponse(
    string PhoneNumber,
    bool IsPrimary
);
