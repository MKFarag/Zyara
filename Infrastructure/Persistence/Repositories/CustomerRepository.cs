namespace Infrastructure.Persistence.Repositories;

public class CustomerRepository(ApplicationDbContext context) : GenericRepository<Customer, string>(context), ICustomerRepository
{
    private readonly ApplicationDbContext _context = context;

    #region Address

    public async Task<IEnumerable<Address>> GetAllAddressesAsync(string customerId, CancellationToken cancellationToken = default)
        => await _context.Addresses
            .AsNoTracking()
            .Where(a => a.CustomerId == customerId)
            .ToListAsync(cancellationToken);

    public async Task<Address?> GetAddressAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Addresses.FindAsync([id], cancellationToken);

    public async Task AddAsync(Address address, CancellationToken cancellationToken = default)
        => await _context.Addresses.AddAsync(address, cancellationToken);

    public void Delete(Address address)
        => _context.Addresses.Remove(address);

    public async Task BulkRemoveIsDefaultAddressAsync(Customer customer, CancellationToken cancellationToken = default)
        => await _context.Addresses
            .Where(a => a.Id == customer.DefaultAddressId)
            .ExecuteUpdateAsync(a =>
                a.SetProperty(x => x.IsDefault, false)
                , cancellationToken
            );

    #endregion




}
