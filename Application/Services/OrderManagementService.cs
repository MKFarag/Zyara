namespace Application.Services;

public class OrderManagementService(IUnitOfWork unitOfWork) : IOrderManagementService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly int _developedYear = 2025;

    public async Task<IEnumerable<OrderResponse>> GetAllByStatusAsync(OrderStatusRequest request, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<OrderStatus>(request.Status, true, out var status))
            return [];

        var orders = await _unitOfWork.Orders
            .FindAllAsync
            (   o => o.Status == status,
                [$"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}"],
                cancellationToken
            );

        return orders.Adapt<IEnumerable<OrderResponse>>();
    }

    public async Task<Result<OrderDetailsResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _unitOfWork.Orders
            .FindAsync
            (
                o => o.Id == id,
                [$"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}", nameof(Order.DeliveryMan)],
                cancellationToken
            );

        if (order is null)
            return Result.Failure<OrderDetailsResponse>(OrderErrors.NotFound);

        var user = await _unitOfWork.Users.FindByIdAsync(order.CustomerId, cancellationToken);

        var phoneNumber = await _unitOfWork.Customers.GetPrimaryPhoneNumberAsync(order.CustomerId, cancellationToken);

        var response = (order, user, phoneNumber).Adapt<OrderDetailsResponse>();

        return Result.Success(response);
    }

    public async Task<Result> ChangeStatusAsync(int id, OrderStatusRequest request, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<OrderStatus>(request.Status, true, out var status))
            return Result.Failure(OrderErrors.InvalidStatus);

        if (await _unitOfWork.Orders.GetAsync(id, cancellationToken) is not { } order)
            return Result.Failure(OrderErrors.NotFound);

        order.Status = status;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> SetDeliveryManAsync(int orderId, int deliveryManId, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Orders.GetAsync(orderId, cancellationToken) is not { } order)
            return Result.Failure(OrderErrors.NotFound);

        if (await _unitOfWork.DeliveryMen.GetAsync(deliveryManId, cancellationToken) is not { } deliveryMan)
            return Result.Failure(DeliveryManErrors.NotFound);

        if (deliveryMan.IsDisabled)
            return Result.Failure(DeliveryManErrors.Disabled);

        order.DeliveryManId = deliveryManId;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }
    
    //public async Task<Result> GetHistoryAsync(RequestFilters filters, DateOnly date, CancellationToken cancellationToken = default)
    //{
    //    if (date.Year < _developedYear)
    //        return Result.Success();
    //}

    //public async Task<Result> GetEarningAsync()
    //{

    //}
}
