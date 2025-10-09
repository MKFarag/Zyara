namespace Application.Services;

public class OrderService(IUnitOfWork unitOfWork) : IOrderService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly int _developedYear = 2025;

    public async Task<IEnumerable<OrderResponse>> GetAllAsync(string customerId, int year, CancellationToken cancellationToken = default)
    {
        if (year < _developedYear)
            return [];

        var orders = await _unitOfWork.Orders
            .FindAllAsync
            (
                o => o.CustomerId == customerId && o.OrderDate.Year == year,
                [$"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}.{nameof(Product.Images)}"],
                cancellationToken
            );

        return orders.Adapt<IEnumerable<OrderResponse>>(); 
    }
   
    public async Task<Result<OrderResponse>> GetAsync(string customerId, int orderId, CancellationToken cancellationToken = default)
    {
        var order = await _unitOfWork.Orders
            .FindAsync
            (
                o => o.Id == orderId,
                [$"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}.{nameof(Product.Images)}"],
                cancellationToken
            );

        if (order is null)
            return Result.Failure<OrderResponse>(OrderErrors.NotFound);

        if (order.CustomerId != customerId)
            return Result.Failure<OrderResponse>(OrderErrors.AccessDenied);

        var response = order.Adapt<OrderResponse>();

        return Result.Success(response); 
    }

    public async Task<Result<OrderStatusResponse>> TrackAsync(string customerId, int orderId, CancellationToken cancellationToken = default)
    {
        var order = await _unitOfWork.Orders
            .FindAsync
            (
                o => o.Id == orderId,
                [nameof(Order.DeliveryMan)],
                cancellationToken
            );

        if (order is null)
            return Result.Failure<OrderStatusResponse>(OrderErrors.NotFound);

        if (order.CustomerId != customerId)
            return Result.Failure<OrderStatusResponse>(OrderErrors.AccessDenied);

        var response = order.Adapt<OrderStatusResponse>();

        return Result.Success(response);
    }

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

        if (customer.CartItems.Any(x => !x.Product.IsAvailable))
            return Result.Failure(ProductErrors.NotAvailable);

        var shippingCost = CalculateShippingCost(totalPrice);

        var order = new Order
        {
            CustomerId = customer.Id,
            TotalAmount = totalPrice,
            ShippingCost = shippingCost,
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

    public async Task<Result> CancelAsync(string customerId, int orderId, CancellationToken cancellationToken = default)
    {
        var order = await _unitOfWork.Orders
            .TrackedFindAsync
            (
                o => o.Id == orderId,
                [$"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}"],
                cancellationToken
            );

        if (order is null)
            return Result.Failure(OrderErrors.NotFound);

        if (order.CustomerId != customerId)
            return Result.Failure(OrderErrors.AccessDenied);

        if (order.Status is not OrderStatus.Pending)
            return Result.Failure(OrderErrors.CannotBeCancelled);

        order.Status = OrderStatus.Canceled;

        foreach (var item in order.OrderItems)
            item.Product.StorageQuantity += item.Quantity;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    private static decimal CalculateShippingCost(decimal orderTotal)
    {
        const decimal baseFee = 25m;
        const decimal percentage = 0.03m;

        var shippingCost = baseFee + (orderTotal * percentage);
        return Math.Round(shippingCost, 2);
    }
}
