namespace Application.Contracts.Order;

public record OrderDetailsResponse(
    CustomerResponse Customer,
    OrderResponse Order,
    OrderStatusResponse Status
);
