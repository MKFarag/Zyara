using Application.Contracts.Order;

namespace Application.Interfaces.Application;

public interface IOrderService
{
    Task<Result> PlaceOrderAsync(string customerId, CancellationToken cancellationToken = default);
}
