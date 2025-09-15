#region Usings

using Application.Contracts.Customer.Address;
using Application.Contracts.Customer.PhoneNumber;
using System.Collections.Generic;

#endregion

namespace Application.Services;

public class CustomerService(IUnitOfWork unitOfWork) : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    #region Address

    public async Task<Result<IEnumerable<AddressResponse>>> GetAllAddressesAsync(string customerId, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Customers.FindAsync(c => c.Id == customerId, [nameof(Customer.Addresses)], cancellationToken) is not { } customer)
            return Result.Failure<IEnumerable<AddressResponse>>(CustomerErrors.NotFound);

        if (customer.Addresses.Count == 0)
            return Result.Failure<IEnumerable<AddressResponse>>(CustomerErrors.Address.NotFound);

        return Result.Success(customer.Addresses.Adapt<IEnumerable<AddressResponse>>());
    }

    public async Task<Result<AddressResponse>> GetDefaultAddressAsync(string customerId, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Customers.FindAsync(c => c.Id == customerId, [nameof(Customer.Addresses)], cancellationToken) is not { } customer)
            return Result.Failure<AddressResponse>(CustomerErrors.NotFound);

        var defaultAddress = customer.Addresses.FirstOrDefault(a => a.IsDefault);

        if (defaultAddress is null)
            return Result.Failure<AddressResponse>(CustomerErrors.Address.NotFound);

        return Result.Success(defaultAddress.Adapt<AddressResponse>());
    }

    public async Task<Result> SetDefaultAddressAsync(string customerId, int addressId, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Customers.TrackedFindAsync(c => c.Id == customerId, [nameof(Customer.Addresses)], cancellationToken) is not { } customer)
            return Result.Failure(CustomerErrors.NotFound);

        var newDefaultAddress = customer.Addresses.FirstOrDefault(a => a.Id == addressId);
        var oldDefaultAddress = customer.Addresses.FirstOrDefault(a => a.IsDefault);

        if (newDefaultAddress is null)
            return Result.Failure(CustomerErrors.Address.NotFound);

        if (oldDefaultAddress is not null)
        {
            if (oldDefaultAddress.Id == newDefaultAddress.Id)
                return Result.Failure(CustomerErrors.Address.AlreadyDefault);

            oldDefaultAddress.IsDefault = false;
        }

        newDefaultAddress.IsDefault = true;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<AddressResponse>> AddAddressAsync(string customerId, AddressRequest request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Customers.TrackedFindAsync(c => c.Id == customerId, [nameof(Customer.Addresses)], cancellationToken) is not { } customer)
            return Result.Failure<AddressResponse>(CustomerErrors.NotFound);

        var address = request.Adapt<Address>();
        address.CustomerId = customerId;

        if (request.IsDefault)
        {
            var oldDefaultAddress = customer.Addresses.FirstOrDefault(a => a.IsDefault);

            if (oldDefaultAddress is not null)
                oldDefaultAddress.IsDefault = false;
        }

        customer.Addresses.Add(address);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(address.Adapt<AddressResponse>());
    }

    public async Task<Result> UpdateAddressAsync(string customerId, int addressId, UpdateAddressRequest request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Customers.TrackedFindAsync(c => c.Id == customerId, [nameof(Customer.Addresses)], cancellationToken) is not { } customer)
            return Result.Failure(CustomerErrors.NotFound);

        var address = customer.Addresses.FirstOrDefault(a => a.Id == addressId);

        if (address is null)
            return Result.Failure(CustomerErrors.Address.NotFound);

        request.Adapt(address);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAddressAsync(string customerId, int addressId, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Customers.FindAsync(c => c.Id == customerId, [nameof(Customer.Addresses)], cancellationToken) is not { } customer)
            return Result.Failure(CustomerErrors.NotFound);

        var address = customer.Addresses.FirstOrDefault(a => a.Id == addressId);

        if (address is null)
            return Result.Failure(CustomerErrors.Address.NotFound);

        if (address.IsDefault)
            return Result.Failure(CustomerErrors.Address.DeleteDefault);

        _unitOfWork.Customers.Delete(address);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    #endregion

    #region PhoneNumber

    public async Task<Result<IEnumerable<PhoneNumberResponse>>> GetAllPhoneNumbersAsync(string customerId, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Customers.FindAsync(c => c.Id == customerId, [nameof(Customer.PhoneNumbers)], cancellationToken) is not { } customer)
            return Result.Failure<IEnumerable<PhoneNumberResponse>>(CustomerErrors.NotFound);

        if (customer.PhoneNumbers.Count == 0)
            return Result.Failure<IEnumerable<PhoneNumberResponse>>(CustomerErrors.PhoneNumber.NotFound);

        IEnumerable<PhoneNumberResponse> response = [];

        foreach (var phoneNumber in customer.PhoneNumbers)
            response = response.Append(new(phoneNumber.PhoneNumber, phoneNumber.IsPrimary));

        return Result.Success(response);
    } 

    public async Task<Result<string>> GetPrimaryPhoneNumber(string customerId, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Customers.FindAsync(c => c.Id == customerId, [nameof(Customer.PhoneNumbers)], cancellationToken) is not { } customer)
            return Result.Failure<string>(CustomerErrors.NotFound);

        if (customer.PhoneNumbers.Count == 0)
            return Result.Failure<string>(CustomerErrors.PhoneNumber.NotFound);

        var response = customer.PhoneNumbers.FirstOrDefault(p => p.IsPrimary);

        return response is null
            ? Result.Failure<string>(CustomerErrors.PhoneNumber.NotFound)
            : Result.Success(response.PhoneNumber);
    }

    public async Task<Result> SetPrimaryPhoneNumberAsync(string customerId, string phoneNumber, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Customers.TrackedFindAsync(c => c.Id == customerId, [nameof(Customer.PhoneNumbers)], cancellationToken) is not { } customer)
            return Result.Failure(CustomerErrors.NotFound);

        if (customer.PhoneNumbers.Count == 0)
            return Result.Failure(CustomerErrors.PhoneNumber.NotFound);

        var oldPrimary = customer.PhoneNumbers.FirstOrDefault(p => p.IsPrimary);

        if (oldPrimary?.PhoneNumber == phoneNumber)
            return Result.Failure(CustomerErrors.PhoneNumber.AlreadyPrimary);

        if (oldPrimary is not null)
            oldPrimary.IsPrimary = false;

        var newPrimary = customer.PhoneNumbers.FirstOrDefault(p => p.PhoneNumber == phoneNumber);

        if (newPrimary is null)
            return Result.Failure(CustomerErrors.PhoneNumber.NotFound);

        newPrimary.IsPrimary = true;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result<PhoneNumberResponse>> AddPhoneNumberAsync(string customerId, string phoneNumber, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Customers.TrackedFindAsync(c => c.Id == customerId, [nameof(Customer.PhoneNumbers)], cancellationToken) is not { } customer)
            return Result.Failure<PhoneNumberResponse>(CustomerErrors.NotFound);

        if (customer.PhoneNumbers.Any(p => p.PhoneNumber == phoneNumber))
            return Result.Failure<PhoneNumberResponse>(CustomerErrors.PhoneNumber.Duplicated);

        var newPhoneNumber = new CustomerPhoneNumber
        {
            CustomerId = customerId,
            PhoneNumber = phoneNumber,
            IsPrimary = customer.PhoneNumbers.Count == 0
        };

        customer.PhoneNumbers ??= [];
        customer.PhoneNumbers.Add(newPhoneNumber);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(newPhoneNumber.Adapt<PhoneNumberResponse>());
    }

    public async Task<Result> DeletePhoneNumberAsync(string customerId, string phoneNumber, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Customers.FindAsync(c => c.Id == customerId, [nameof(Customer.PhoneNumbers)], cancellationToken) is not { } customer)
            return Result.Failure(CustomerErrors.NotFound);

        if (customer.PhoneNumbers.Count == 0)
            return Result.Failure(CustomerErrors.PhoneNumber.NotFound);

        var deletedPhoneNumber = customer.PhoneNumbers.FirstOrDefault(p => p.PhoneNumber == phoneNumber);

        if (deletedPhoneNumber is null)
            return Result.Failure(CustomerErrors.PhoneNumber.NotFound);
        
        if (deletedPhoneNumber.IsPrimary)
            return Result.Failure(CustomerErrors.PhoneNumber.DeletePrimary);

        _unitOfWork.Customers.Delete(deletedPhoneNumber);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    #endregion
}
