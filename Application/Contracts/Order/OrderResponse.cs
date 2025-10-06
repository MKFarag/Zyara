namespace Application.Contracts.Order;

public record OrderResponse(
    int Id,
    DateOnly OrderDate,
    decimal? ShippingCost,
    decimal TotalAmount,
    decimal GrandTotal,
    string ShippingAddress,
    IEnumerable<OrderItemResponse> OrderItems
);
