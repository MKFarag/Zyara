namespace Application.Services;

public class OrderManagementService(IUnitOfWork unitOfWork) : IOrderManagementService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    private readonly int _developedYear = 2025;
    private static readonly string _allowedSearchColumn = nameof(Order.CustomerId);
    private static readonly HashSet<string> _allowedSortColumns = [nameof(Order.Id), nameof(Order.TotalAmount)];

    public async Task<IPaginatedList<OrderResponse>> GetAllByStatusAsync(RequestFilters filters, OrderStatusRequest request, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<OrderStatus>(request.Status, true, out var status))
            return EmptyPaginatedList.Create<OrderResponse>();

        var checkedFilters = filters.Check(_allowedSortColumns);

        var orders = await _unitOfWork.Orders
            .FindPaginatedListAsync<OrderResponse>
            (   
                o => o.Status == status,
                checkedFilters.PageNumber,
                checkedFilters.PageSize,
                checkedFilters.SearchValue,
                _allowedSearchColumn,
                checkedFilters.SortColumn!,
                checkedFilters.SortDirection!,
                ColumnType.String,
                [$"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}"],
                cancellationToken
            );

        return orders;
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
    
    public async Task<IPaginatedList<OrderManagementResponse>> GetCurrentHistoryAsync(RequestFilters filters, CancellationToken cancellationToken = default)
    {
        var checkedFilters = filters.Check(_allowedSortColumns);

        var orders = await _unitOfWork.Orders
            .FindPaginatedListAsync<OrderManagementResponse>
            (
                o => o.OrderDate.Date == DateTime.UtcNow.Date,
                checkedFilters.PageNumber,
                checkedFilters.PageSize,
                checkedFilters.SearchValue,
                _allowedSearchColumn,
                checkedFilters.SortColumn!,
                checkedFilters.SortDirection!,
                ColumnType.String,
                cancellationToken
            );

        return orders;
    }

    public async Task<IPaginatedList<OrderManagementResponse>> GetHistoryByDateAsync(RequestFilters filters, DateOnly date, CancellationToken cancellationToken = default)
    {
        if (date.Year < _developedYear)
            return EmptyPaginatedList.Create<OrderManagementResponse>();

        var checkedFilters = filters.Check(_allowedSortColumns);

        var orders = await _unitOfWork.Orders
            .FindPaginatedListAsync<OrderManagementResponse>
            (
                o => DateOnly.FromDateTime(o.OrderDate) == date,
                checkedFilters.PageNumber,
                checkedFilters.PageSize,
                checkedFilters.SearchValue,
                _allowedSearchColumn,
                checkedFilters.SortColumn!,
                checkedFilters.SortDirection!,
                ColumnType.String,
                cancellationToken
            );

        return orders;
    }

    public async Task<IPaginatedList<OrderManagementResponse>> GetHistoryByMonthAsync(RequestFilters filters, int month, CancellationToken cancellationToken = default)
    {
        if (month > 12 || month < 0)
            return EmptyPaginatedList.Create<OrderManagementResponse>();

        var checkedFilters = filters.Check(_allowedSortColumns);

        var orders = await _unitOfWork.Orders
            .FindPaginatedListAsync<OrderManagementResponse>
            (
                o => o.OrderDate.Month == month && o.OrderDate.Year == DateTime.UtcNow.Year,
                checkedFilters.PageNumber,
                checkedFilters.PageSize,
                checkedFilters.SearchValue,
                _allowedSearchColumn,
                checkedFilters.SortColumn!,
                checkedFilters.SortDirection!,
                ColumnType.String,
                cancellationToken
            );

        return orders;
    }

    public async Task<IPaginatedList<OrderManagementResponse>> GetHistoryByYearAsync(RequestFilters filters, int year, CancellationToken cancellationToken = default)
    {
        if (year < _developedYear)
            return EmptyPaginatedList.Create<OrderManagementResponse>();

        var checkedFilters = filters.Check(_allowedSortColumns);

        var orders = await _unitOfWork.Orders
            .FindPaginatedListAsync<OrderManagementResponse>
            (
                o => o.OrderDate.Year == year,
                checkedFilters.PageNumber,
                checkedFilters.PageSize,
                checkedFilters.SearchValue,
                _allowedSearchColumn,
                checkedFilters.SortColumn!,
                checkedFilters.SortDirection!,
                ColumnType.String,
                cancellationToken
            );

        return orders;
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
}
