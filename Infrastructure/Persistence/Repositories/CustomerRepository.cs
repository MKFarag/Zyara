namespace Infrastructure.Persistence.Repositories;

public class CustomerRepository(ApplicationDbContext context) : GenericRepository<Customer, string>(context), ICustomerRepository
{
    private readonly ApplicationDbContext _context = context;

    public void Delete(Address address)
        => _context.Addresses.Remove(address);

    public void Delete(CustomerPhoneNumber customerPhoneNumber)
        => _context.CustomerPhoneNumbers.Remove(customerPhoneNumber);

}
