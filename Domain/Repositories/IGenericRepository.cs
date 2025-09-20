namespace Domain.Repositories;

/// <summary>Generic repository interface for CRUD operations</summary>
/// <typeparam name="TEntity">The entity type</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key</typeparam>
public interface IGenericRepository<TEntity, TKey> : IBasicRepository<TEntity>
    where TEntity : class
    where TKey : notnull
{
    #region Read

    #region Get

    /// <summary>Get trackable entity by ID</summary>
    /// <param name="id">Entity ID</param>
    TEntity? Get(TKey id);

    /// <summary>Get trackable entity by ID</summary>
    /// <param name="id">Entity ID</param>
    Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>Get all entities</summary>
    IEnumerable<TEntity> GetAll();

    /// <summary>Get all entities</summary>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Get all trackable entities</summary>
    IEnumerable<TEntity> TrackedGetAll();

    /// <summary>Get all trackable entities</summary>
    Task<IEnumerable<TEntity>> TrackedGetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Get all entities</summary>
    /// <param name="includes">Related data to include</param>
    IEnumerable<TEntity> GetAll(string[] includes);

    /// <summary>Get all entities</summary>
    /// <param name="includes">Related data to include</param>
    Task<IEnumerable<TEntity>> GetAllAsync(string[] includes, CancellationToken cancellationToken = default);

    /// <summary>Get all trackable entities</summary>
    /// <param name="includes">Related data to include</param>
    IEnumerable<TEntity> TrackedGetAll(string[] includes);

    /// <summary>Get all trackable entities</summary>
    /// <param name="includes">Related data to include</param>
    Task<IEnumerable<TEntity>> TrackedGetAllAsync(string[] includes, CancellationToken cancellationToken = default);

    #endregion

    #region Find

    /// <summary>Find first entity matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    TEntity? Find(Expression<Func<TEntity, bool>> predicate);

    /// <summary>Find first trackable entity matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    TEntity? TrackedFind(Expression<Func<TEntity, bool>> predicate);

    /// <summary>Find first entity matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="includes">Related data to include</param>
    TEntity? Find(Expression<Func<TEntity, bool>> predicate, string[] includes);

    /// <summary>Find first trackable entity matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="includes">Related data to include</param>
    TEntity? TrackedFind(Expression<Func<TEntity, bool>> predicate, string[] includes);

    /// <summary>Find first trackable entity matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="includes">Related data to include</param>
    Task<TEntity?> TrackedFindAsync(Expression<Func<TEntity, bool>> predicate, string[] includes, CancellationToken cancellationToken = default);

    /// <summary>Find all entities matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);

    /// <summary>Find all trackable entities matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    IEnumerable<TEntity> TrackedFindAll(Expression<Func<TEntity, bool>> predicate);

    /// <summary>Find all entities matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="includes">Related data to include</param>
    IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, string[] includes);

    /// <summary>Find all trackable entities matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="includes">Related data to include</param>
    IEnumerable<TEntity> TrackedFindAll(Expression<Func<TEntity, bool>> predicate, string[] includes);

    /// <summary>Find all trackable entities matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="includes">Related data to include</param>
    Task<IEnumerable<TEntity>> TrackedFindAllAsync(Expression<Func<TEntity, bool>> predicate, string[] includes, CancellationToken cancellationToken = default);

    /// <summary>Find entities with ordering and pagination</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="take">Number to take</param>
    /// <param name="skip">Number to skip</param>
    /// <param name="orderBy">Order by expression</param>
    /// <param name="orderByType">Sort direction</param>
    IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, int? take = null, int? skip = null, Expression<Func<TEntity, object>>? orderBy = null, string orderByType = OrderBy.Ascending);

    /// <summary>Find entities with ordering and pagination</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="take">Number to take</param>
    /// <param name="skip">Number to skip</param>
    /// <param name="orderBy">Order by expression</param>
    /// <param name="orderByType">Sort direction</param>
    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, int? take = null, int? skip = null, Expression<Func<TEntity, object>>? orderBy = null, string orderByType = OrderBy.Ascending, CancellationToken cancellationToken = default);

    #endregion

    #region Projection

    #region Get

    /// <summary>Get all entities converted to DTO</summary>
    /// <typeparam name="TProjection">The type of the projection</typeparam>
    IEnumerable<TProjection> GetAllProjection<TProjection>() where TProjection : class;

    /// <summary>Get all entities converted to DTO</summary>
    /// <typeparam name="TProjection">The type of the projection</typeparam>
    Task<IEnumerable<TProjection>> GetAllProjectionAsync<TProjection>(CancellationToken cancellationToken = default) where TProjection : class;

    /// <summary>Get all entities converted to DTO</summary>
    /// <typeparam name="TProjection">The type of the projection</typeparam>
    /// <param name="includes">Related data to include</param>
    IEnumerable<TProjection> GetAllProjection<TProjection>(string[] includes) where TProjection : class;

    /// <summary>Get all entities converted to DTO</summary>
    /// <typeparam name="TProjection">The type of the projection</typeparam>
    /// <param name="includes">Related data to include</param>
    Task<IEnumerable<TProjection>> GetAllProjectionAsync<TProjection>(string[] includes, CancellationToken cancellationToken = default) where TProjection : class;

    /// <summary>Get all entities with custom projection</summary>
    /// <param name="selector">Custom projection expression</param>
    /// <param name="distinct">Apply distinct to results</param>
    IEnumerable<TProjection> GetAllProjection<TProjection>(Expression<Func<TEntity, TProjection>> selector, bool distinct);

    /// <summary>Get all entities with custom projection</summary>
    /// <param name="selector">Custom projection expression</param>
    /// <param name="distinct">Apply distinct to results</param>
    Task<IEnumerable<TProjection>> GetAllProjectionAsync<TProjection>(Expression<Func<TEntity, TProjection>> selector, bool distinct, CancellationToken cancellationToken = default);

    /// <summary>Get all entities with custom projection</summary>
    /// <param name="selector">Custom projection expression</param>
    /// <param name="distinct">Apply distinct to results</param>
    /// <param name="includes">Related data to include</param>
    IEnumerable<TProjection> GetAllProjection<TProjection>(string[] includes, Expression<Func<TEntity, TProjection>> selector, bool distinct);

    /// <summary>Get all entities with custom projection</summary>
    /// <param name="selector">Custom projection expression</param>
    /// <param name="distinct">Apply distinct to results</param>
    /// <param name="includes">Related data to include</param>
    Task<IEnumerable<TProjection>> GetAllProjectionAsync<TProjection>(string[] includes, Expression<Func<TEntity, TProjection>> selector, bool distinct, CancellationToken cancellationToken = default);

    #endregion

    #region Find

    /// <summary>Find all entities with your DTO</summary>
    /// <param name="predicate">Filter condition</param>
    IEnumerable<TProjection> FindAllProjection<TProjection>(Expression<Func<TEntity, bool>> predicate) where TProjection : class;

    /// <summary>Find all entities with your DTO</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IEnumerable<TProjection>> FindAllProjectionAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) where TProjection : class;

    /// <summary>Find all entities converted to DTO</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="includes">Related data to include</param>
    IEnumerable<TProjection> FindAllProjection<TProjection>(Expression<Func<TEntity, bool>> predicate, string[] includes) where TProjection : class;

    /// <summary>Find all entities converted to DTO</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="includes">Related data to include</param
    Task<IEnumerable<TProjection>> FindAllProjectionAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, string[] includes, CancellationToken cancellationToken = default) where TProjection : class;

    /// <summary>Find all entities with custom projection</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="selector">Custom projection expression</param>
    /// <param name="distinct">Apply distinct to results</param>
    IEnumerable<TProjection> FindAllProjection<TProjection>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjection>> selector, bool distinct);

    /// <summary>Find all entities with custom projection</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="selector">Custom projection expression</param>
    /// <param name="distinct">Apply distinct to results</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IEnumerable<TProjection>> FindAllProjectionAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjection>> selector, bool distinct, CancellationToken cancellationToken = default);

    /// <summary>Find all entities with custom projection</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="includes">Related data to include</param>
    /// <param name="selector">Custom projection expression</param>
    /// <param name="distinct">Apply distinct to results</param>
    IEnumerable<TProjection> FindAllProjection<TProjection>(Expression<Func<TEntity, bool>> predicate, string[] includes, Expression<Func<TEntity, TProjection>> selector, bool distinct);

    /// <summary>Find all entities with custom projection</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="includes">Related data to include</param>
    /// <param name="selector">Custom projection expression</param>
    /// <param name="distinct">Apply distinct to results</param>
    Task<IEnumerable<TProjection>> FindAllProjectionAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, string[] includes, Expression<Func<TEntity, TProjection>> selector, bool distinct, CancellationToken cancellationToken = default);

    #endregion

    #endregion

    #endregion

    #region Modify

    /// <summary>Create new entity</summary>
    /// <param name="entity">Entity to add</param>
    TEntity Add(TEntity entity);

    /// <summary>Create multiple entities</summary>
    /// <param name="entities">Entities to add</param>
    IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);

    #endregion

    #region Helpers

    /// <summary>Count all entities</summary>
    int Count();

    /// <summary>Count all entities</summary>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>Count entities matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    int Count(Expression<Func<TEntity, bool>> predicate);

    /// <summary>Count entities matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>Check if any entities match condition</summary>
    /// <param name="predicate">Filter condition</param>
    bool Any(Expression<Func<TEntity, bool>> predicate);

    /// <summary>Check if entity exists by ID</summary>
    /// <param name="id">Entity ID</param>
    bool Exists(TKey id);

    /// <summary>Check if entity exists by ID</summary>
    /// <param name="id">Entity ID</param>
    Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>Attach entity to context for tracking</summary>
    /// <param name="entity">Entity to attach</param>
    void Attach(TEntity entity);

    /// <summary>Attach multiple entities to context for tracking</summary>
    /// <param name="entities">Entities to attach</param>
    void AttachRange(IEnumerable<TEntity> entities);

    /// <summary>Mark a property as modified</summary>
    /// <param name="entity">Entity to mark property as modified</param>
    /// <param name="property">Property to mark as modified</param>
    void MarkPropertyAsModified<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property);

    #endregion
}
