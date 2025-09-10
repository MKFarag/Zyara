namespace Domain.Repositories;

public interface ICustomerRepository : IGenericRepository<Customer, string>
{
    #region Address

    Task<IEnumerable<Address>> GetAllAddressesAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Address?> GetAddressAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(Address address, CancellationToken cancellationToken = default);
    void Delete(Address address);
    Task BulkRemoveIsDefaultAddressAsync(Customer customer, CancellationToken cancellationToken = default);

    #endregion

}
