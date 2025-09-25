namespace Application.Contracts.DeliveryMan;

public record DeliveryManResponse(
    int Id,
    string Name,
    string PhoneNumber,
    bool IsDisabled
);
