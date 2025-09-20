using Application.Contracts.Cart;

namespace Application.Interfaces.Application;

public interface ICartService
{
    Task<Result<CartResponse>> GetAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Result> ClearAsync(string customerId, CancellationToken cancellationToken = default);
}
