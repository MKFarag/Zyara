namespace Infrastructure.Persistence.Repositories;

public class CustomerRepository(ApplicationDbContext context) : GenericRepository<Customer, string>(context), ICustomerRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<string?> GetPrimaryPhoneNumberAsync(string id, CancellationToken cancellationToken = default)
        => await _context.CustomerPhoneNumbers
            .AsNoTracking()
            .Where(p => p.IsPrimary && p.CustomerId == id)
            .Select(p => p.PhoneNumber)
            .FirstOrDefaultAsync(cancellationToken);

    public void Delete(Address address)
        => _context.Addresses.Remove(address);

    public void Delete(CustomerPhoneNumber customerPhoneNumber)
        => _context.CustomerPhoneNumbers.Remove(customerPhoneNumber);

}
