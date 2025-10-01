using System.Linq.Dynamic.Core;

namespace Infrastructure.Persistence.Repositories;

public class GenericRepositoryWithPagination<TEntity, TKey>(ApplicationDbContext context) 
    : GenericRepository<TEntity, TKey>(context), IGenericRepositoryWithPagination<TEntity, TKey>
    where TEntity : class
    where TKey : notnull
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task<IPaginatedList<TProjection>> GetPaginatedListAsync<TProjection>(
        int pageNumber, int pageSize, string? searchValue, string? searchColumn, string sortColumn, string sortDirection,
        ColumnType searchColumnType, CancellationToken cancellationToken = default) where TProjection : class
    {
        var query = _dbSet.AsNoTracking().AsQueryable();

        query = ApplySearchFilter(query, searchValue, searchColumn, searchColumnType);

        var finalQuery = query.OrderBy($"{sortColumn} {sortDirection}").ProjectToType<TProjection>();

        return await PaginatedList<TProjection>.CreateAsync(finalQuery, pageNumber, pageSize, cancellationToken);
    }

    public async Task<IPaginatedList<TProjection>> GetPaginatedListAsync<TProjection>(
        int pageNumber, int pageSize, string? searchValue, string? searchColumn, string sortColumn, string sortDirection,
        ColumnType searchColumnType, string[] includes, CancellationToken cancellationToken = default) where TProjection : class
    {
        var query = _dbSet.AsNoTracking().AsQueryable();

        query = ApplySearchFilter(query, searchValue, searchColumn, searchColumnType);

        foreach (var include in includes)
            query = query.Include(include);

        var finalQuery = query.OrderBy($"{sortColumn} {sortDirection}").ProjectToType<TProjection>();

        return await PaginatedList<TProjection>.CreateAsync(finalQuery, pageNumber, pageSize, cancellationToken);
    }

    public async Task<IPaginatedList<TProjection>> FindPaginatedListAsync<TProjection>(
        Expression<Func<TEntity, bool>> predicate,
        int pageNumber, int pageSize, string? searchValue, string? searchColumn, string sortColumn, string sortDirection,
        ColumnType searchColumnType, CancellationToken cancellationToken = default) where TProjection : class
    {
        var query = _dbSet.AsNoTracking().Where(predicate).AsQueryable();

        query = ApplySearchFilter(query, searchValue, searchColumn, searchColumnType);

        var finalQuery = query.OrderBy($"{sortColumn} {sortDirection}").ProjectToType<TProjection>();

        return await PaginatedList<TProjection>.CreateAsync(finalQuery, pageNumber, pageSize, cancellationToken);
    }

    public async Task<IPaginatedList<TProjection>> FindPaginatedListAsync<TProjection>(
        Expression<Func<TEntity, bool>> predicate,
        int pageNumber, int pageSize, string? searchValue, string? searchColumn, string sortColumn, string sortDirection,
        ColumnType searchColumnType, string[] includes, CancellationToken cancellationToken = default) where TProjection : class
    {
        var query = _dbSet.AsNoTracking().Where(predicate).AsQueryable();

        query = ApplySearchFilter(query, searchValue, searchColumn, searchColumnType);

        foreach (var include in includes)
            query = query.Include(include);

        var finalQuery = query.OrderBy($"{sortColumn} {sortDirection}").ProjectToType<TProjection>();

        return await PaginatedList<TProjection>.CreateAsync(finalQuery, pageNumber, pageSize, cancellationToken);
    }

    private static IQueryable<T> ApplySearchFilter<T>(IQueryable<T> query, string? searchValue, string? searchColumn, ColumnType searchColumnType)
    {
        if (string.IsNullOrEmpty(searchValue) || string.IsNullOrEmpty(searchColumn))
            return query;

        switch (searchColumnType)
        {
            case ColumnType.String:
                query = query.Where($"{searchColumn}.Contains(@0)", searchValue);
                break;

            case ColumnType.Int:
                if (int.TryParse(searchValue, out var intValue))
                    query = query.Where($"{searchColumn} == @0", intValue);
                break;

            case ColumnType.Bool:
                if (bool.TryParse(searchValue, out var boolValue))
                    query = query.Where($"{searchColumn} == @0", boolValue);
                break;

            case ColumnType.Date:
                if (DateTime.TryParse(searchValue, out var dateValue))
                    query = query.Where($"{searchColumn}.Date == @0", dateValue.Date);
                break;
        }

        return query;
    }
}
