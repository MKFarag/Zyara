using Application.Contracts.Cart;

namespace Application.Services;

public class CartService(IUnitOfWork unitOfWork) : ICartService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<CartResponse>> GetAsync(string customerId, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Customers.ExistsAsync(customerId, cancellationToken))
            return Result.Failure<CartResponse>(CustomerErrors.NotFound);

        var cart = await _unitOfWork.Carts
            .FindAllAsync(c => c.CustomerId == customerId, [nameof(Cart.Product)], cancellationToken);

        var totalPrice = cart.Sum(c => c.Product.CurrentPrice * c.Quantity);

        cart = [.. cart.OrderByDescending(c => c.AddedAt)];

        var response = new CartResponse
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

        return Result.Success(response);
    }

    public async Task<Result> AddAsync(string customerId, int productId, int quantity, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Customers.ExistsAsync(customerId, cancellationToken))
            return Result.Failure(CustomerErrors.NotFound);

        if (await _unitOfWork.Products.GetAsync(productId, cancellationToken) is not { } product)
            return Result.Failure(ProductErrors.NotFound);

        if (!product.IsAvailable)
            return Result.Failure(ProductErrors.NotAvailable);

        if (quantity > product.StorageQuantity)
            return Result.Failure(CustomerErrors.Cart.QuantityExceedsStock);

        var cart = await _unitOfWork.Carts
            .TrackedFindAsync(c => c.CustomerId == customerId && c.ProductId == productId, cancellationToken);

        if (cart is null)
        {
            cart = new Cart
            {
                CustomerId = customerId,
                ProductId = productId,
                Quantity = quantity
            };

            await _unitOfWork.Carts.AddAsync(cart, cancellationToken);
        }
        else
            cart.Quantity += quantity;

        if (cart.Quantity > product.StorageQuantity)
            return Result.Failure(CustomerErrors.Cart.QuantityExceedsStock);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(string customerId, int productId, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Customers.ExistsAsync(customerId, cancellationToken))
            return Result.Failure(CustomerErrors.NotFound);

        if (!await _unitOfWork.Products.ExistsAsync(productId, cancellationToken))
            return Result.Failure(ProductErrors.NotFound);

        if (!await _unitOfWork.Carts.AnyAsync(c => c.CustomerId == customerId && c.ProductId == productId, cancellationToken))
            return Result.Failure(CustomerErrors.Cart.ProductNotFound);

        await _unitOfWork.Carts.ExecuteDeleteAsync(c => c.CustomerId == customerId && c.ProductId == productId, cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(string customerId, int productId, int quantity, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Customers.ExistsAsync(customerId, cancellationToken))
            return Result.Failure(CustomerErrors.NotFound);

        if (!await _unitOfWork.Products.ExistsAsync(productId, cancellationToken))
            return Result.Failure(ProductErrors.NotFound);

        if (await _unitOfWork.Carts
            .TrackedFindAsync(c => c.CustomerId == customerId && c.ProductId == productId, cancellationToken) is not { } cart)
            return Result.Failure(CustomerErrors.Cart.ProductNotFound);

        cart.Quantity -= quantity;

        switch (cart.Quantity)
        {
            case < 0:
                return Result.Failure(CustomerErrors.Cart.QuantityLessThanZero);
            case 0:
                await _unitOfWork.Carts.ExecuteDeleteAsync(c => c.CustomerId == customerId && c.ProductId == productId, cancellationToken);
                break;
            default:
                await _unitOfWork.CompleteAsync(cancellationToken);
                break;
        }

        return Result.Success();
    }

    public async Task<Result> ClearAsync(string customerId, CancellationToken cancellationToken = default)
    {
        if (!await _unitOfWork.Customers.ExistsAsync(customerId, cancellationToken))
            return Result.Failure(CustomerErrors.NotFound);

        await _unitOfWork.Carts.ExecuteDeleteAsync(c => c.CustomerId == customerId, cancellationToken);

        return Result.Success();
    }
}
