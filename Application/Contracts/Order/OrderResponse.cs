namespace Application.Contracts.Order;

public record OrderResponse(
    int Id,
    DateTime OrderDate,
    decimal? ShippingCost,
    decimal TotalAmount,
    string ShippingAddress,
    IEnumerable<OrderItemResponse> OrderItems
);
