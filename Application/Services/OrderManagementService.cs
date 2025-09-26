namespace Application.Services;

public class OrderManagementService(IUnitOfWork unitOfWork) : IOrderManagementService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<OrderResponse>> GetAllByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default)
    {
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
}
