using Application.Contracts.Product;

namespace Application.Services;

public class ProductService(IUnitOfWork unitOfWork) : IProductService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    private static readonly string _allowedSearchColumn = nameof(Product.Name);

    private static readonly HashSet<string> _allowedSortColumns = new(StringComparer.OrdinalIgnoreCase) 
    { nameof(Product.Id), nameof(Product.CurrentPrice) };

    public async Task<IPaginatedList<ProductResponse>> GetAllAsync(RequestFilters filters, CancellationToken cancellationToken = default)
    {
        var (sortColumn, sortDirection) = FiltersCheck(filters);

        return await _unitOfWork.Products
            .GetPaginatedListAsync<ProductResponse>
            (
                filters.PageNumber,
                filters.PageSize,
                filters.SearchValue,
                _allowedSearchColumn,
                sortColumn,
                sortDirection,
                cancellationToken
            );
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
