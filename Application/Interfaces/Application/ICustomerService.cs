using Application.Contracts.Customer.Address;

namespace Application.Interfaces.Application;

public interface ICustomerService
{
    #region Address

    Task<Result<IEnumerable<AddressResponse>>> GetAllAddressesAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Result<AddressResponse>> GetAddressAsync(int addressId, CancellationToken cancellationToken = default);
    Task<Result<AddressResponse>> AddAddressAsync(string customerId, AddressRequest request, CancellationToken cancellationToken = default);
    Task<Result> SetDefaultAddressAsync(string customerId, int addressId, CancellationToken cancellationToken = default);
    Task<Result> UpdateAddressAsync(int addressId, UpdateAddressRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAddressAsync(string customerId, int addressId, CancellationToken cancellationToken = default);

    #endregion

}
