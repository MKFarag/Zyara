namespace Domain.Repositories;

public interface ICustomerRepository : IGenericRepository<Customer, string>
{
    void Delete(Address address);
    void Delete(CustomerPhoneNumber customerPhoneNumber);
}
