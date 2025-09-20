using Application.Contracts.Cart;

namespace Application.Services;

public class CartService(IUnitOfWork unitOfWork) : ICartService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CartResponse> GetAsync(string customerId, CancellationToken cancellationToken = default)
    {
        var cart = await _unitOfWork.Carts
            .FindAllAsync(c => c.CustomerId == customerId, [nameof(Cart.Product)], cancellationToken);

        var totalPrice = cart.Sum(c => c.Product.CurrentPrice * c.Quantity);

        cart = [.. cart.OrderByDescending(c => c.AddedAt)];

        return new CartResponse
        (
            [.. cart.Select(c => new CartProductResponse
            (
                c.ProductId,
                c.Product.Name,
                c.Product.CurrentPrice,
                c.Quantity
            ))],
            totalPrice
        );
    }

    public Task ClearAsync(string customerId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
