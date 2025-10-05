namespace Application.Contracts.DeliveryMan;

public record DeliveryManDetailResponse(
    int Id,
    string Name,
    string PhoneNumber,
    bool IsDisabled,
    int TotalOrders,
    IEnumerable<string> OrdersId
);