namespace Domain.Repositories;

public interface ICartRepository
{
    Task<IEnumerable<Cart>> FindAllAsync(Expression<Func<Cart, bool>> predicate, string[] includes, CancellationToken cancellationToken = default);
    Task BulkDeleteAsync(Expression<Func<Cart, bool>> predicate, CancellationToken cancellationToken = default);
}
