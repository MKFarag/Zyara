namespace Application.Interfaces.Application;

public interface IOrderManagementService
{
    Task<IPaginatedList<OrderResponse>> GetAllByStatusAsync(RequestFilters filters, OrderStatusRequest request, CancellationToken cancellationToken = default);
    Task<Result<OrderDetailsResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> ChangeStatusAsync(int id, OrderStatusRequest request, CancellationToken cancellationToken = default);
    Task<Result> SetDeliveryManAsync(int orderId, int deliveryManId, CancellationToken cancellationToken = default);
    Task<IPaginatedList<OrderManagementResponse>> GetCurrentHistoryAsync(RequestFilters filters, CancellationToken cancellationToken = default);
    Task<IPaginatedList<OrderManagementResponse>> GetHistoryByDateAsync(RequestFilters filters, DateOnly date, CancellationToken cancellationToken = default);
    Task<IPaginatedList<OrderManagementResponse>> GetHistoryByMonthAsync(RequestFilters filters, int month, CancellationToken cancellationToken = default);
    Task<IPaginatedList<OrderManagementResponse>> GetHistoryByYearAsync(RequestFilters filters, int year, CancellationToken cancellationToken = default);
    Task<Result<OrderEarningResponse>> GetCurrentEarningAsync(CancellationToken cancellationToken = default);
    Task<Result<OrderEarningResponse>> GetEarningByDateAsync(DateOnly date, CancellationToken cancellationToken = default);
    Task<Result<OrderEarningResponse>> GetEarningByMonthAsync(int month, CancellationToken cancellationToken = default);
    Task<Result<OrderEarningResponse>> GetEarningByYearAsync(int year, CancellationToken cancellationToken = default);
}
