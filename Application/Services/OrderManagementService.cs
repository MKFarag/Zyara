namespace Application.Services;

public class OrderManagementService(IUnitOfWork unitOfWork) : IOrderManagementService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    private readonly int _developedYear = 2025;
    private static readonly string _allowedSearchColumn = nameof(Order.CustomerId);
    private static readonly HashSet<string> _allowedSortColumns = [nameof(Order.Id), nameof(Order.TotalAmount)];

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
    
    public async Task<Result<IPaginatedList<OrderManagementResponse>>> GetCurrentHistoryAsync(RequestFilters filters, CancellationToken cancellationToken = default)
    {
        var (sortColumn, sortDirection) = FiltersCheck(filters);

        var orders = await _unitOfWork.Orders
            .FindPaginatedListAsync<OrderManagementResponse>
            (
                o => o.OrderDate.Date == DateTime.UtcNow.Date,
                filters.PageNumber,
                filters.PageSize,
                filters.SearchValue,
                _allowedSearchColumn,
                sortColumn,
                sortDirection,
                cancellationToken
            );

        return Result.Success(orders);
    }

    public async Task<Result<IPaginatedList<OrderManagementResponse>>> GetHistoryByDateAsync(RequestFilters filters, DateOnly date, CancellationToken cancellationToken = default)
    {
        if (date.Year < _developedYear)
            return Result.Failure<IPaginatedList<OrderManagementResponse>>(OrderErrors.InvalidInput);

        var (sortColumn, sortDirection) = FiltersCheck(filters);

        var orders = await _unitOfWork.Orders
            .FindPaginatedListAsync<OrderManagementResponse>
            (
                o => DateOnly.FromDateTime(o.OrderDate) == date,
                filters.PageNumber,
                filters.PageSize,
                filters.SearchValue,
                _allowedSearchColumn,
                sortColumn,
                sortDirection,
                cancellationToken
            );

        return Result.Success(orders);
    }

    public async Task<Result<IPaginatedList<OrderManagementResponse>>> GetHistoryByMonthAsync(RequestFilters filters, int month, CancellationToken cancellationToken = default)
    {
        if (month > 12 || month < 0)
            return Result.Failure<IPaginatedList<OrderManagementResponse>>(OrderErrors.InvalidInput);

        var (sortColumn, sortDirection) = FiltersCheck(filters);

        var orders = await _unitOfWork.Orders
            .FindPaginatedListAsync<OrderManagementResponse>
            (
                o => o.OrderDate.Month == month && o.OrderDate.Year == DateTime.UtcNow.Year,
                filters.PageNumber,
                filters.PageSize,
                filters.SearchValue,
                _allowedSearchColumn,
                sortColumn,
                sortDirection,
                cancellationToken
            );

        return Result.Success(orders);
    }

    public async Task<Result<IPaginatedList<OrderManagementResponse>>> GetHistoryByYearAsync(RequestFilters filters, int year, CancellationToken cancellationToken = default)
    {
        if (year < _developedYear)
            return Result.Failure<IPaginatedList<OrderManagementResponse>>(OrderErrors.InvalidInput);

        var (sortColumn, sortDirection) = FiltersCheck(filters);

        var orders = await _unitOfWork.Orders
            .FindPaginatedListAsync<OrderManagementResponse>
            (
                o => o.OrderDate.Year == year,
                filters.PageNumber,
                filters.PageSize,
                filters.SearchValue,
                _allowedSearchColumn,
                sortColumn,
                sortDirection,
                cancellationToken
            );

        return Result.Success(orders);
    }

    public async Task<Result<OrderEarningResponse>> GetCurrentEarningAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _unitOfWork.Orders
            .FindAllProjectionAsync
            (
                o => o.OrderDate.Date == DateTime.UtcNow.Date,
                o => o.TotalAmount,
                false,
                cancellationToken
            );

        var totalEarning = orders.Sum();

        return Result.Success(new OrderEarningResponse(totalEarning));
    }

    public async Task<Result<OrderEarningResponse>> GetEarningByDateAsync(DateOnly date, CancellationToken cancellationToken = default)
    {
        if (date.Year < _developedYear)
            return Result.Success(new OrderEarningResponse(0));

        var orders = await _unitOfWork.Orders
            .FindAllProjectionAsync
            (
                o => DateOnly.FromDateTime(o.OrderDate) == date,
                o => o.TotalAmount,
                false,
                cancellationToken
            );

        var totalEarning = orders.Sum();

        return Result.Success(new OrderEarningResponse(totalEarning));
    }

    public async Task<Result<OrderEarningResponse>> GetEarningByMonthAsync(int month, CancellationToken cancellationToken = default)
    {
        if (month > 12 || month < 0)
            return Result.Failure<OrderEarningResponse>(OrderErrors.InvalidInput);

        var orders = await _unitOfWork.Orders
            .FindAllProjectionAsync
            (
                o => o.OrderDate.Month == month && o.OrderDate.Year == DateTime.UtcNow.Year,
                o => o.TotalAmount,
                false,
                cancellationToken
            );

        var totalEarning = orders.Sum();

        return Result.Success(new OrderEarningResponse(totalEarning));
    }

    public async Task<Result<OrderEarningResponse>> GetEarningByYearAsync(int year, CancellationToken cancellationToken = default)
    {
        if (year < _developedYear)
            return Result.Success(new OrderEarningResponse(0));

        var orders = await _unitOfWork.Orders
            .FindAllProjectionAsync
            (
                o => o.OrderDate.Year == year,
                o => o.TotalAmount,
                false,
                cancellationToken
            );

        var totalEarning = orders.Sum();

        return Result.Success(new OrderEarningResponse(totalEarning));
    }

    private static (string sortColumn, string sortDirection) FiltersCheck(RequestFilters filters)
    {
        string sortColumn, sortDirection;

        if (!string.IsNullOrEmpty(filters.SortColumn))
            sortColumn = _allowedSortColumns
                .FirstOrDefault(x => string.Equals(x, filters.SortColumn, StringComparison.OrdinalIgnoreCase))
                ?? _allowedSortColumns.First();
        else
            sortColumn = _allowedSortColumns.First();

        if (!(string.Equals(filters.SortDirection, OrderBy.Ascending, StringComparison.OrdinalIgnoreCase)
            || string.Equals(filters.SortDirection, OrderBy.Descending, StringComparison.OrdinalIgnoreCase)))
            sortDirection = OrderBy.Ascending;
        else
            sortDirection = filters.SortDirection!;

        return (sortColumn, sortDirection);
    }
}
