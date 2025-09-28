namespace Application.Interfaces.Application;

public interface IOrderManagementService
{
    Task<IEnumerable<OrderResponse>> GetAllByStatusAsync(OrderStatusRequest request, CancellationToken cancellationToken = default);
    Task<Result<OrderDetailsResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> ChangeStatusAsync(int id, OrderStatusRequest request, CancellationToken cancellationToken = default);
    Task<Result> SetDeliveryManAsync(int orderId, int deliveryManId, CancellationToken cancellationToken = default);
    Task<Result<IPaginatedList<OrderManagementResponse>>> GetCurrentHistoryAsync(RequestFilters filters, CancellationToken cancellationToken = default);
    Task<Result<IPaginatedList<OrderManagementResponse>>> GetHistoryByDateAsync(RequestFilters filters, DateOnly date, CancellationToken cancellationToken = default);
    Task<Result<IPaginatedList<OrderManagementResponse>>> GetHistoryByMonthAsync(RequestFilters filters, int month, CancellationToken cancellationToken = default);
    Task<Result<IPaginatedList<OrderManagementResponse>>> GetHistoryByYearAsync(RequestFilters filters, int year, CancellationToken cancellationToken = default);
}
