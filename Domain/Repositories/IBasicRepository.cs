namespace Domain.Repositories;

/// <summary>Basic repository interface for CRUD operations</summary>
/// <typeparam name="TEntity">The entity type</typeparam>
public interface IBasicRepository<TEntity> where TEntity : class
{
    #region Read

    /// <summary>Find first entity matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>Find first entity matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="includes">Related data to include</param>
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, string[] includes, CancellationToken cancellationToken = default);

    /// <summary>Find all entities matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>Find all entities matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    /// <param name="includes">Related data to include</param>
    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, string[] includes, CancellationToken cancellationToken = default);

    /// <summary>Find first trackable entity matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    Task<TEntity?> TrackedFindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>Find all trackable entities matching condition</summary>
    /// <param name="predicate">Filter condition</param>
    Task<IEnumerable<TEntity>> TrackedFindAllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    #endregion

    #region Modify

    /// <summary>Create new entity</summary>
    /// <param name="entity">Entity to add</param>
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>Create multiple entities</summary>
    /// <param name="entities">Entities to add</param>
    Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>Delete entity</summary>
    /// <param name="entity">Entity to delete</param>
    void Delete(TEntity entity);

    /// <summary>Delete multiple entities</summary>
    /// <param name="entities">Entities to delete</param>
    void DeleteRange(IEnumerable<TEntity> entities);

    /// <summary>Execute delete command directly in the database for entities matching the predicate</summary>
    /// <param name="predicate">Filter condition</param>
    Task ExecuteDeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>Update entity</summary>
    /// <param name="entity">Entity to update</param>
    TEntity Update(TEntity entity);

    /// <summary>Update multiple entities</summary>
    /// <param name="entities">Entities to update</param>
    IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities);

    /// <summary>Check if any entities match condition</summary>
    /// <param name="predicate">Filter condition</param>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    #endregion
}

