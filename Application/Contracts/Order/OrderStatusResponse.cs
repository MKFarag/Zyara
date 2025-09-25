namespace Application.Contracts.Order;

public record OrderStatusResponse(
    string Status,
    DeliveryManResponse? DeliveryMan
);
