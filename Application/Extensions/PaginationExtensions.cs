namespace Application.Extensions;

internal static class PaginationExtensions
{
    internal static RequestFilters Check(this RequestFilters filters, HashSet<string> allowedSortColumns)
    {
        string sortColumn, sortDirection;

        if (!string.IsNullOrEmpty(filters.SortColumn))
            sortColumn = allowedSortColumns
                .FirstOrDefault(x => string.Equals(x, filters.SortColumn, StringComparison.OrdinalIgnoreCase))
                ?? allowedSortColumns.First();
        else
            sortColumn = allowedSortColumns.First();

        if (!OrderBy.IsValid(filters.SortDirection))
            sortDirection = OrderBy.Ascending;
        else
            sortDirection = filters.SortDirection!;

        var newFilters = filters with
        {
            SortColumn = sortColumn,
            SortDirection = sortDirection
        };

        return newFilters;
    }
}
