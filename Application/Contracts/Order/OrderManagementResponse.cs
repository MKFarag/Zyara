namespace Application.Contracts.Order;

public record OrderManagementResponse(
    int Id,
    string CustomerId,
    DateOnly OrderDate,
    int ItemCount,
    decimal TotalAmount
);
