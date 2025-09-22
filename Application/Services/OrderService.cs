using Application.Contracts.Order;

namespace Application.Services;

public class OrderService(IUnitOfWork unitOfWork) : IOrderService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> PlaceOrderAsync(string customerId, CancellationToken cancellationToken = default)
    {
        var customer = await _unitOfWork.Customers
            .TrackedFindAsync
            (
                c => c.Id == customerId,
                [
                    nameof(Customer.Addresses),
                    $"{nameof(Customer.CartItems)}.{nameof(Cart.Product)}",
                    nameof(Customer.PhoneNumbers)
                ],
                cancellationToken
            );

        if (customer is null)
            return Result.Failure(CustomerErrors.NotFound);

        if (customer.Addresses.FirstOrDefault(a => a.IsDefault) is not { } defaultAddress)
            return Result.Failure(CustomerErrors.Address.NotFound);

        if (!customer.PhoneNumbers.Any(p => p.IsPrimary))
            return Result.Failure(CustomerErrors.PhoneNumber.NotFound);

        if (customer.CartItems.Count == 0)
            return Result.Failure(OrderErrors.EmptyCart);

        var totalPrice = customer.CartItems.Sum(c => c.Product.CurrentPrice * c.Quantity);

        var order = new Order
        {
            CustomerId = customer.Id,
            TotalAmount = totalPrice,
            Status = OrderStatus.Pending,
            ShippingAddress = defaultAddress.ToString()
        };

        await _unitOfWork.Orders.AddAsync(order, cancellationToken);

        order.OrderItems = [.. customer.CartItems.Select(c => new OrderItem
        {
            Order = order,
            ProductId = c.ProductId,
            Quantity = c.Quantity,
            UnitPrice = c.Product.CurrentPrice
        })];

        foreach (var cartItem in customer.CartItems)
        {
            cartItem.Product.StorageQuantity -= cartItem.Quantity;

            if (cartItem.Product.StorageQuantity < 0)
                return Result.Failure(CustomerErrors.Cart.QuantityExceedsStock);
        }

        _unitOfWork.Carts.DeleteRange(customer.CartItems);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }
}
