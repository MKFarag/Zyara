namespace Domain.Repositories;

public interface ICustomerRepository : IGenericRepository<Customer>
{
    Task<string?> GetPrimaryPhoneNumberAsync(string id, CancellationToken cancellationToken = default);
    void Delete(Address address);
    void Delete(CustomerPhoneNumber customerPhoneNumber);
}
