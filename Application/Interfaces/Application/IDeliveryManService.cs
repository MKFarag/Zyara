namespace Application.Interfaces.Application;

public interface IDeliveryManService
{
    Task<IEnumerable<DeliveryManResponse>> GetAllAsync(bool includeDisabled, CancellationToken cancellationToken = default);
    Task<Result<DeliveryManDetailResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<DeliveryManOrderResponse>>> GetOrdersAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<DeliveryManResponse>> AddAsync(DeliveryManRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, DeliveryManRequest request, CancellationToken cancellationToken = default);
    Task<Result> SetToOrderAsync(int deliveryManId, int orderId, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(int id, CancellationToken cancellationToken = default);
}
