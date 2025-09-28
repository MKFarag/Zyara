using Domain.Entities;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private bool _disposed = false;

    public IGenericRepository<Address, int> Addresses { get; private set; }
    public IBasicRepository<Cart> Carts { get; private set; }
    public ICustomerRepository Customers { get; private set; }
    public IGenericRepository<DeliveryMan, int> DeliveryMen { get; private set; }
    public IGenericRepositoryWithPagination<Order, int> Orders { get; private set; }
    public IGenericRepository<OrderItem, int> OrderItems { get; private set; }
    public IGenericRepositoryWithPagination<Product, int> Products { get; private set; }
    public IRoleRepository Roles { get; private set; }
    public IUserRepository Users { get; private set; }

    public UnitOfWork(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _context = context;

        Addresses = new GenericRepository<Address, int>(_context);
        Carts = new BasicRepository<Cart>(_context);
        Customers = new CustomerRepository(_context);
        DeliveryMen = new GenericRepository<DeliveryMan, int>(_context);
        Orders = new GenericRepositoryWithPagination<Order, int>(_context);
        OrderItems = new GenericRepositoryWithPagination<OrderItem, int>(_context);
        Products = new GenericRepositoryWithPagination<Product, int>(_context);
        Roles = new RoleRepository(_context, roleManager);
        Users = new UserRepository(_context, userManager);
    }

    public int Complete()
        => _context.SaveChanges();

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
                _context?.Dispose();

            _disposed = true;
        }
    }
}