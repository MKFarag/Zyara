#region Usings

using Application.Contracts.Customer.Address;
using Application.Contracts.Customer.PhoneNumber; 

#endregion

namespace Application.Interfaces.Application;

public interface ICustomerService
{
    #region Address

    Task<Result<IEnumerable<AddressResponse>>> GetAllAddressesAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Result<AddressResponse>> GetDefaultAddressAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Result> SetDefaultAddressAsync(string customerId, int addressId, CancellationToken cancellationToken = default);
    Task<Result<AddressResponse>> AddAddressAsync(string customerId, AddressRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAddressAsync(string customerId, int addressId, UpdateAddressRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAddressAsync(string customerId, int addressId, CancellationToken cancellationToken = default);

    #endregion

    #region PhoneNumber

    Task<Result<IEnumerable<PhoneNumberResponse>>> GetAllPhoneNumbersAsync(string customerId, CancellationToken cancellationToken = default);
    Task<Result<string>> GetPrimaryPhoneNumber(string customerId, CancellationToken cancellationToken = default);
    Task<Result> SetPrimaryPhoneNumberAsync(string customerId, string phoneNumber, CancellationToken cancellationToken = default);
    Task<Result<PhoneNumberResponse>> AddPhoneNumberAsync(string customerId, string phoneNumber, CancellationToken cancellationToken = default);
    Task<Result> DeletePhoneNumberAsync(string customerId, string phoneNumber, CancellationToken cancellationToken = default);

    #endregion

}
