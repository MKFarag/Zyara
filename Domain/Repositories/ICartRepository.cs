namespace Domain.Repositories;

public interface ICartRepository
{
    Task<IEnumerable<Cart>> FindAllAsync(Expression<Func<Cart, bool>> predicate, string[] includes, CancellationToken cancellationToken = default);
    //Task<Cart?> FindAsync(Expression<Func<Cart, bool>> predicate, CancellationToken cancellationToken = default);
    //Task<Cart?> FindAsync(Expression<Func<Cart, bool>> predicate, string[] includes, CancellationToken cancellationToken = default);
    Task ExecuteDeleteAsync(Expression<Func<Cart, bool>> predicate, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<Cart, bool>> predicate, CancellationToken cancellationToken = default);
}
