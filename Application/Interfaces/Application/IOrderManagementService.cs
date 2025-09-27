namespace Application.Interfaces.Application;

public interface IOrderManagementService
{
    Task<IEnumerable<OrderResponse>> GetAllByStatusAsync(OrderStatusRequest request, CancellationToken cancellationToken = default);
    Task<Result<OrderDetailsResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
}
