namespace Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity, TKey>(ApplicationDbContext context) : BasicRepository<TEntity>(context), IGenericRepository<TEntity, TKey>
    where TEntity : class
    where TKey : notnull
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    private readonly ApplicationDbContext _context = context;
    private const int _maxIncludeDepth = 2;
    private const int _maxIncludeCount = 3;

    #region Read

    #region Get

    public TEntity? Get(TKey id)
        => _dbSet.Find(id);

    public async Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default)
        => await _dbSet.FindAsync([id], cancellationToken);

    public IEnumerable<TEntity> GetAll()
        => [.. _dbSet.AsNoTracking()];

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbSet.AsNoTracking().ToListAsync(cancellationToken);

    public IEnumerable<TEntity> TrackedGetAll()
        => [.. _dbSet];

    public async Task<IEnumerable<TEntity>> TrackedGetAllAsync(CancellationToken cancellationToken = default)
        => await _dbSet.ToListAsync(cancellationToken);

    public IEnumerable<TEntity> GetAll(string[] includes)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        foreach (var include in includes)
            query = query.Include(include);

        if (includes.Any(x => x.Count(c => c == '.') >= _maxIncludeDepth) || includes.Length >= _maxIncludeCount)
            query = query.AsSingleQuery();

        return query;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(string[] includes, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        foreach (var include in includes)
            query = query.Include(include);

        if (includes.Any(x => x.Count(c => c == '.') >= _maxIncludeDepth) || includes.Length >= _maxIncludeCount)
            query = query.AsSingleQuery();

        return await query.ToListAsync(cancellationToken);
    }

    public IEnumerable<TEntity> TrackedGetAll(string[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        foreach (var include in includes)
            query = query.Include(include);

        if (includes.Any(x => x.Count(c => c == '.') >= _maxIncludeDepth) || includes.Length >= _maxIncludeCount)
            query = query.AsSingleQuery();

        return query;
    }

    public async Task<IEnumerable<TEntity>> TrackedGetAllAsync(string[] includes, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet;

        foreach (var include in includes)
            query = query.Include(include);

        if (includes.Any(x => x.Count(c => c == '.') >= _maxIncludeDepth) || includes.Length >= _maxIncludeCount)
            query = query.AsSingleQuery();

        return await query.ToListAsync(cancellationToken);
    }

    #endregion

    #region Find

    public TEntity? Find(Expression<Func<TEntity, bool>> predicate)
        => _dbSet.AsNoTracking().FirstOrDefault(predicate);

    public TEntity? TrackedFind(Expression<Func<TEntity, bool>> predicate)
        => _dbSet.FirstOrDefault(predicate);

    public TEntity? Find(Expression<Func<TEntity, bool>> predicate, string[] includes)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking().Where(predicate);

        foreach (var include in includes)
            query = query.Include(include);

        return query.FirstOrDefault();
    }

    public TEntity? TrackedFind(Expression<Func<TEntity, bool>> predicate, string[] includes)
    {
        IQueryable<TEntity> query = _dbSet.Where(predicate);

        foreach (var include in includes)
            query = query.Include(include);

        return query.FirstOrDefault();
    }

    public async Task<TEntity?> TrackedFindAsync(Expression<Func<TEntity, bool>> predicate, string[] includes, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet.Where(predicate);

        foreach (var include in includes)
            query = query.Include(include);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        => _dbSet.AsNoTracking().Where(predicate);

    public IEnumerable<TEntity> TrackedFindAll(Expression<Func<TEntity, bool>> predicate)
        => _dbSet.Where(predicate);

    public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, string[] includes)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking().Where(predicate);

        foreach (var include in includes)
            query = query.Include(include);

        return query;
    }

    public IEnumerable<TEntity> TrackedFindAll(Expression<Func<TEntity, bool>> predicate, string[] includes)
    {
        IQueryable<TEntity> query = _dbSet.Where(predicate);

        foreach (var include in includes)
            query = query.Include(include);

        return [.. query];
    }

    public async Task<IEnumerable<TEntity>> TrackedFindAllAsync(Expression<Func<TEntity, bool>> predicate, string[] includes, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet.Where(predicate);

        foreach (var include in includes)
            query = query.Include(include);

        return await query.ToListAsync(cancellationToken);
    }

    public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, int? take = null, int? skip = null, Expression<Func<TEntity, object>>? orderBy = null, string orderByType = OrderBy.Ascending)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking().Where(predicate);

        if (orderBy != null)
            if (string.Equals(orderByType, OrderBy.Ascending, StringComparison.OrdinalIgnoreCase))
                query = query.OrderBy(orderBy);
            else if (string.Equals(orderByType, OrderBy.Descending, StringComparison.OrdinalIgnoreCase))
                query = query.OrderByDescending(orderBy);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return query;
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, int? take = null, int? skip = null, Expression<Func<TEntity, object>>? orderBy = null, string orderByType = OrderBy.Ascending, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking().Where(predicate);

        if (orderBy != null)
            if (string.Equals(orderByType, OrderBy.Ascending, StringComparison.OrdinalIgnoreCase))
                query = query.OrderBy(orderBy);
            else if (string.Equals(orderByType, OrderBy.Descending, StringComparison.OrdinalIgnoreCase))
                query = query.OrderByDescending(orderBy);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return await query.ToListAsync(cancellationToken);
    }

    #endregion

    #region Projection

    #region Get

    public IEnumerable<TProjection> GetAllProjection<TProjection>() where TProjection : class
        => [.. _dbSet.AsNoTracking().ProjectToType<TProjection>()];

    public async Task<IEnumerable<TProjection>> GetAllProjectionAsync<TProjection>(CancellationToken cancellationToken = default) where TProjection : class
        => await _dbSet.AsNoTracking().ProjectToType<TProjection>().ToListAsync(cancellationToken);

    public IEnumerable<TProjection> GetAllProjection<TProjection>(string[] includes) where TProjection : class
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        foreach (var include in includes)
            query = query.Include(include);

        return [.. query.ProjectToType<TProjection>()];
    }

    public async Task<IEnumerable<TProjection>> GetAllProjectionAsync<TProjection>(string[] includes, CancellationToken cancellationToken = default) where TProjection : class
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        foreach (var include in includes)
            query = query.Include(include);

        return await query.ProjectToType<TProjection>().ToListAsync(cancellationToken);
    }

    public IEnumerable<TProjection> GetAllProjection<TProjection>(Expression<Func<TEntity, TProjection>> selector, bool distinct)
        => distinct
            ? _dbSet.AsNoTracking().Select(selector).Distinct()
            : _dbSet.AsNoTracking().Select(selector);

    public async Task<IEnumerable<TProjection>> GetAllProjectionAsync<TProjection>(Expression<Func<TEntity, TProjection>> selector, bool distinct, CancellationToken cancellationToken = default)
        => distinct
            ? await _dbSet.AsNoTracking().Select(selector).Distinct().ToListAsync(cancellationToken)
            : await _dbSet.AsNoTracking().Select(selector).ToListAsync(cancellationToken);

    public IEnumerable<TProjection> GetAllProjection<TProjection>(string[] includes, Expression<Func<TEntity, TProjection>> selector, bool distinct)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        foreach (var include in includes)
            query = query.Include(include);

        var newQuery = query.Select(selector);

        return distinct
            ? [.. newQuery.Distinct()]
            : [.. newQuery];
    }

    public async Task<IEnumerable<TProjection>> GetAllProjectionAsync<TProjection>(string[] includes, Expression<Func<TEntity, TProjection>> selector, bool distinct, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        foreach (var include in includes)
            query = query.Include(include);

        var newQuery = query.Select(selector);

        return distinct
            ? await newQuery.Distinct().ToListAsync(cancellationToken)
            : await newQuery.ToListAsync(cancellationToken);
    }

    #endregion

    #region Find

    public IEnumerable<TProjection> FindAllProjection<TProjection>(Expression<Func<TEntity, bool>> predicate) where TProjection : class
        => _dbSet.AsNoTracking().Where(predicate).ProjectToType<TProjection>();

    public async Task<IEnumerable<TProjection>> FindAllProjectionAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) where TProjection : class
        => await _dbSet.AsNoTracking().Where(predicate).ProjectToType<TProjection>().ToListAsync(cancellationToken);

    public IEnumerable<TProjection> FindAllProjection<TProjection>(Expression<Func<TEntity, bool>> predicate, string[] includes) where TProjection : class
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking().Where(predicate);

        foreach (var include in includes)
            query = query.Include(include);

        return [.. query.ProjectToType<TProjection>()];
    }

    public async Task<IEnumerable<TProjection>> FindAllProjectionAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, string[] includes, CancellationToken cancellationToken = default) where TProjection : class
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking().Where(predicate);

        foreach (var include in includes)
            query = query.Include(include);

        return await query.ProjectToType<TProjection>().ToListAsync(cancellationToken);
    }

    public IEnumerable<TProjection> FindAllProjection<TProjection>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjection>> selector, bool distinct)
        => distinct
            ? _dbSet.AsNoTracking().Where(predicate).Select(selector).Distinct()
            : _dbSet.AsNoTracking().Where(predicate).Select(selector);

    public async Task<IEnumerable<TProjection>> FindAllProjectionAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjection>> selector, bool distinct, CancellationToken cancellationToken = default)
        => distinct
            ? await _dbSet.AsNoTracking().Where(predicate).Select(selector).Distinct().ToListAsync(cancellationToken)
            : await _dbSet.AsNoTracking().Where(predicate).Select(selector).ToListAsync(cancellationToken);

    public IEnumerable<TProjection> FindAllProjection<TProjection>(Expression<Func<TEntity, bool>> predicate, string[] includes, Expression<Func<TEntity, TProjection>> selector, bool distinct)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking().Where(predicate);

        foreach (var include in includes)
            query = query.Include(include);

        var newQuery = query.Select(selector);

        return distinct
            ? [.. newQuery.Distinct()]
            : [.. newQuery];
    }

    public async Task<IEnumerable<TProjection>> FindAllProjectionAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, string[] includes, Expression<Func<TEntity, TProjection>> selector, bool distinct, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking().Where(predicate);

        foreach (var include in includes)
            query = query.Include(include);

        var newQuery = query.Select(selector);

        return distinct
            ? await newQuery.Distinct().ToListAsync(cancellationToken)
            : await newQuery.ToListAsync(cancellationToken);
    }

    #endregion

    #endregion 

    #endregion

    #region Modify

    public TEntity Add(TEntity entity)
    {
        _dbSet.Add(entity);
        return entity;
    }

    public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
        return entities;
    }

    #endregion

    #region Helpers

    public int Count()
        => _dbSet.Count();

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        => await _dbSet.CountAsync(cancellationToken);

    public int Count(Expression<Func<TEntity, bool>> predicate)
        => _dbSet.Count(predicate);

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await _dbSet.CountAsync(predicate, cancellationToken);

    public bool Any(Expression<Func<TEntity, bool>> predicate)
        => _dbSet.Any(predicate);

    public bool Exists(TKey id)
        => _dbSet.Find(id) is not null;

    public async Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
        => await _dbSet.FindAsync([id], cancellationToken) is not null;

    public void Attach(TEntity entity)
        => _dbSet.Attach(entity);

    public void AttachRange(IEnumerable<TEntity> entities)
        => _dbSet.AttachRange(entities);

    public void MarkPropertyAsModified<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property)
    {
        var entry = _context.Entry(entity);

        if (entry.State == EntityState.Detached)
            _dbSet.Attach(entity);

        entry.Property(property).IsModified = true;
    }

    #endregion
}
