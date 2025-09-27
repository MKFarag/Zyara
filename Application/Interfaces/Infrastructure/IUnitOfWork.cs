namespace Application.Interfaces.Infrastructure;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Address, int> Addresses { get; }
    IBasicRepository<Cart> Carts { get; }
    ICustomerRepository Customers { get; }
    IGenericRepository<DeliveryMan, int> DeliveryMen { get; }
    IGenericRepositoryWithPagination<Order, int> Orders { get; }
    IGenericRepository<OrderItem, int> OrderItems { get; }
    IGenericRepositoryWithPagination<Product, int> Products { get; }
    IRoleRepository Roles { get; }
    IUserRepository Users { get; }

    /// <summary>Save changes to the database</summary>
    /// <returns>The number of state entries written to the database</returns>
    int Complete();

    /// <summary>Save changes to the database</summary>
    /// <returns>The number of state entries written to the database</returns>
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}