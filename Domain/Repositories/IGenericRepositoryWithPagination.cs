namespace Domain.Repositories;

public interface IGenericRepositoryWithPagination<TEntity, TKey> : IGenericRepository<TEntity, TKey>, IPaginatedRepository<TEntity>
    where TEntity : class
    where TKey : notnull
{
}
