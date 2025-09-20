namespace Infrastructure.Persistence.Repositories;

public class CartRepository(ApplicationDbContext context) : ICartRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Cart>> FindAllAsync(Expression<Func<Cart, bool>> predicate, string[] includes, CancellationToken cancellationToken = default)
    {
        IQueryable<Cart> query = _context.Carts.AsNoTracking().Where(predicate);

        foreach (var include in includes)
            query = query.Include(include);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task ExecuteDeleteAsync(Expression<Func<Cart, bool>> predicate, CancellationToken cancellationToken = default)
        => await _context.Carts.Where(predicate).ExecuteDeleteAsync(cancellationToken);

    public async Task<bool> AnyAsync(Expression<Func<Cart, bool>> predicate, CancellationToken cancellationToken = default)
        => await _context.Carts.AnyAsync(predicate, cancellationToken);
}
