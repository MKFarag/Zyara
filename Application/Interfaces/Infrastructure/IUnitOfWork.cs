namespace Application.Interfaces.Infrastructure;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Address> Addresses { get; }
    IGenericRepository<Cart> Carts { get; }
    ICustomerRepository Customers { get; }
    IGenericRepository<DeliveryMan> DeliveryMen { get; }
    IGenericRepositoryWithPagination<Order> Orders { get; }
    IGenericRepository<OrderItem> OrderItems { get; }
    IGenericRepositoryWithPagination<Product> Products { get; }
    IRoleRepository Roles { get; }
    IUserRepository Users { get; }

    /// <summary>Save changes to the database</summary>
    /// <returns>The number of state entries written to the database</returns>
    int Complete();

    /// <summary>Save changes to the database</summary>
    /// <returns>The number of state entries written to the database</returns>
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}