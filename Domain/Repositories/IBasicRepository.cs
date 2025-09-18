namespace Domain.Repositories;

/// <summary>Generic repository interface for CRUD operations</summary>
/// <typeparam name="TEntity">The entity type</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key</typeparam>
public interface IBasicRepository<TEntity> where TEntity : class
{
    #region Read

    /// <summary>Get all entities</summary>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Find first entity matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>Find first trackable entity matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<TEntity?> TrackedFindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>Find first entity matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="includes">Related data to include</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, string[] includes, CancellationToken cancellationToken = default);

    /// <summary>Find first trackable entity matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="includes">Related data to include</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<TEntity?> TrackedFindAsync(Expression<Func<TEntity, bool>> predicate, string[] includes, CancellationToken cancellationToken = default);

    /// <summary>Find all entities matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>Find all trackable entities matching condition (async)</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IEnumerable<TEntity>> TrackedFindAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>Find all entities with related data</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="includes">Related data to include</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, string[] includes, CancellationToken cancellationToken = default);

    /// <summary>Get all entities with your DTO</summary>
    /// <typeparam name="TProjection">The type of the projection</typeparam>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IEnumerable<TProjection>> GetAllProjectionAsync<TProjection>(CancellationToken cancellationToken = default) where TProjection : class;

    /// <summary>Get all entities with custom projection</summary>
    /// <param name="selector">Custom projection expression</param>
    /// <param name="distinct">Apply distinct to results</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IEnumerable<TProjection>> GetAllProjectionAsync<TProjection>(Expression<Func<TEntity, TProjection>> selector, bool distinct, CancellationToken cancellationToken = default);

    /// <summary>Find all entities with your DTO</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IEnumerable<TProjection>> FindAllProjectionAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) where TProjection : class;

    /// <summary>Find all entities with custom projection</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="selector">Custom projection expression</param>
    /// <param name="distinct">Apply distinct to results</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IEnumerable<TProjection>> FindAllProjectionAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjection>> selector, bool distinct, CancellationToken cancellationToken = default);

    #endregion

    #region Modify

    /// <summary>Create new entity (async)</summary>
    /// <param name="entity">Entity to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>Create multiple entities (async)</summary>
    /// <param name="entities">Entities to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>Delete entity</summary>
    /// <param name="entity">Entity to delete</param>
    void Delete(TEntity entity);

    /// <summary>Delete multiple entities</summary>
    /// <param name="entities">Entities to delete</param>
    void DeleteRange(IEnumerable<TEntity> entities);

    /// <summary>Update entity</summary>
    /// <param name="entity">Entity to update</param>
    TEntity Update(TEntity entity);

    /// <summary>Update multiple entities</summary>
    /// <param name="entities">Entities to update</param>
    IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities);

    /// <summary>Check if any entities match condition (async)</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default); 

    #endregion
}
