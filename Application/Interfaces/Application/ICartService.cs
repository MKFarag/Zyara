using Application.Contracts.Cart;

namespace Application.Interfaces.Application;

public interface ICartService
{
    Task<CartResponse> GetAsync(string customerId, CancellationToken cancellationToken = default);
}
