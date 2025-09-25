namespace Application.Interfaces.Application;

public interface IOrderService
{
    Task<Result<IEnumerable<OrderResponse>>> GetAllAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Result<OrderResponse>> GetAsync(string customerId, int orderId, CancellationToken cancellationToken = default);
    Task<Result<OrderStatusResponse>> TrackAsync(string customerId, int orderId, CancellationToken cancellationToken = default);
    Task<Result> PlaceOrderAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Result> CancelAsync(string customerId, int orderId, CancellationToken cancellationToken = default);
}
