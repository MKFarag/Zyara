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
        CancellationToken cancellationToken = default) where TProjection : class
    {
        var query = _dbSet.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(searchValue))
            query = query.Where($"{searchColumn}.Contains(@0)", searchValue);

        var finalQuery = query.OrderBy($"{sortColumn} {sortDirection}").ProjectToType<TProjection>();

        return await PaginatedList<TProjection>.CreateAsync(finalQuery, pageNumber, pageSize, cancellationToken);
    }

    public async Task<IPaginatedList<TProjection>> GetPaginatedListAsync<TProjection>(
        int pageNumber, int pageSize, string? searchValue, string? searchColumn, string sortColumn, string sortDirection,
        string[] includes, CancellationToken cancellationToken = default) where TProjection : class
    {
        var query = _dbSet.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(searchValue))
            query = query.Where($"{searchColumn}.Contains(@0)", searchValue);

        foreach (var include in includes)
            query = query.Include(include);

        var finalQuery = query.OrderBy($"{sortColumn} {sortDirection}").ProjectToType<TProjection>();

        return await PaginatedList<TProjection>.CreateAsync(finalQuery, pageNumber, pageSize, cancellationToken);
    }

    public async Task<IPaginatedList<TProjection>> GetPaginatedListAsync<TProjection>(
        int pageNumber, int pageSize, int? searchValue, string? searchColumn, string sortColumn, string sortDirection,
        CancellationToken cancellationToken = default) where TProjection : class
    {
        var query = _dbSet.AsNoTracking().AsQueryable();

        if (searchValue.HasValue)
            query = query.Where($"{searchColumn} == @0", searchValue);

        var finalQuery = query.OrderBy($"{sortColumn} {sortDirection}").ProjectToType<TProjection>();

        return await PaginatedList<TProjection>.CreateAsync(finalQuery, pageNumber, pageSize, cancellationToken);
    }

    public async Task<IPaginatedList<TProjection>> GetPaginatedListAsync<TProjection>(
        int pageNumber, int pageSize, int? searchValue, string? searchColumn, string sortColumn, string sortDirection,
        string[] includes, CancellationToken cancellationToken = default) where TProjection : class
    {
        var query = _dbSet.AsNoTracking().AsQueryable();

        if (searchValue.HasValue)
            query = query.Where($"{searchColumn} == @0", searchValue);

        foreach (var include in includes)
            query = query.Include(include);

        var finalQuery = query.OrderBy($"{sortColumn} {sortDirection}").ProjectToType<TProjection>();

        return await PaginatedList<TProjection>.CreateAsync(finalQuery, pageNumber, pageSize, cancellationToken);
    }

    public async Task<IPaginatedList<TProjection>> FindPaginatedListAsync<TProjection>(
        Expression<Func<TEntity, bool>> predicate,
        int pageNumber, int pageSize, string? searchValue, string? searchColumn, string sortColumn, string sortDirection,
        CancellationToken cancellationToken = default) where TProjection : class
    {
        var query = _dbSet.AsNoTracking().Where(predicate).AsQueryable();

        if (!string.IsNullOrEmpty(searchValue))
            query = query.Where($"{searchColumn}.Contains(@0)", searchValue);

        var finalQuery = query.OrderBy($"{sortColumn} {sortDirection}").ProjectToType<TProjection>();

        return await PaginatedList<TProjection>.CreateAsync(finalQuery, pageNumber, pageSize, cancellationToken);
    }

    public async Task<IPaginatedList<TProjection>> FindPaginatedListAsync<TProjection>(
        Expression<Func<TEntity, bool>> predicate,
        int pageNumber, int pageSize, string? searchValue, string? searchColumn, string sortColumn, string sortDirection,
        string[] includes, CancellationToken cancellationToken = default) where TProjection : class
    {
        var query = _dbSet.AsNoTracking().Where(predicate).AsQueryable();

        if (!string.IsNullOrEmpty(searchValue))
            query = query.Where($"{searchColumn}.Contains(@0)", searchValue);

        foreach (var include in includes)
            query = query.Include(include);

        var finalQuery = query.OrderBy($"{sortColumn} {sortDirection}").ProjectToType<TProjection>();

        return await PaginatedList<TProjection>.CreateAsync(finalQuery, pageNumber, pageSize, cancellationToken);
    }

    public async Task<IPaginatedList<TProjection>> FindPaginatedListAsync<TProjection>(
        Expression<Func<TEntity, bool>> predicate,
        int pageNumber, int pageSize, int? searchValue, string? searchColumn, string sortColumn, string sortDirection,
        CancellationToken cancellationToken = default) where TProjection : class
    {
        var query = _dbSet.AsNoTracking().Where(predicate).AsQueryable();

        if (searchValue.HasValue)
            query = query.Where($"{searchColumn} == @0", searchValue);

        var finalQuery = query.OrderBy($"{sortColumn} {sortDirection}").ProjectToType<TProjection>();

        return await PaginatedList<TProjection>.CreateAsync(finalQuery, pageNumber, pageSize, cancellationToken);
    }

    public async Task<IPaginatedList<TProjection>> FindPaginatedListAsync<TProjection>(
        Expression<Func<TEntity, bool>> predicate,
        int pageNumber, int pageSize, int? searchValue, string? searchColumn, string sortColumn, string sortDirection,
        string[] includes, CancellationToken cancellationToken = default) where TProjection : class
    {
        var query = _dbSet.AsNoTracking().Where(predicate).AsQueryable();

        if (searchValue.HasValue)
            query = query.Where($"{searchColumn} == @0", searchValue);

        foreach (var include in includes)
            query = query.Include(include);

        var finalQuery = query.OrderBy($"{sortColumn} {sortDirection}").ProjectToType<TProjection>();

        return await PaginatedList<TProjection>.CreateAsync(finalQuery, pageNumber, pageSize, cancellationToken);
    }
}
