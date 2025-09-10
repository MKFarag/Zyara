#region Usings

using Application.Contracts.Customer.Address; 

#endregion

namespace Application.Services;

public class CustomerService(IUnitOfWork unitOfWork) : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    #region Address

    public async Task<Result<IEnumerable<AddressResponse>>> GetAllAddressesAsync(string customerId, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Customers.ExistsAsync(customerId, cancellationToken))
            return Result.Failure<IEnumerable<AddressResponse>>(CustomerErrors.NotFound);

        var addresses = await _unitOfWork.Customers.GetAllAddressesAsync(customerId, cancellationToken);

        return Result.Success(addresses.Adapt<IEnumerable<AddressResponse>>());
    }

    public async Task<Result<AddressResponse>> GetAddressAsync(int addressId, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Customers.GetAddressAsync(addressId, cancellationToken) is not { } address)
            return Result.Failure<AddressResponse>(CustomerErrors.Address.NotFound);

        return Result.Success(address.Adapt<AddressResponse>());
    }

    public async Task<Result<AddressResponse>> AddAddressAsync(string customerId, AddressRequest request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Customers.GetAsync(customerId, cancellationToken) is not { } customer)
            return Result.Failure<AddressResponse>(CustomerErrors.NotFound);

        var address = request.Adapt<Address>();
        address.CustomerId = customerId;

        await _unitOfWork.Customers.AddAsync(address, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        if (request.IsDefault)
        {
            if (customer.DefaultAddressId.HasValue)
                await _unitOfWork.Customers.BulkRemoveIsDefaultAddressAsync(customer, cancellationToken);

            customer.DefaultAddress = address;
            customer.DefaultAddressId = address.Id;

            await _unitOfWork.CompleteAsync(cancellationToken);
        }

        return Result.Success(address.Adapt<AddressResponse>());
    }

    public async Task<Result> SetDefaultAddressAsync(string customerId, int addressId, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Customers.GetAsync(customerId, cancellationToken) is not { } customer)
            return Result.Failure(CustomerErrors.NotFound);

        if (customer.DefaultAddressId == addressId)
            return Result.Failure(CustomerErrors.Address.AlreadyDefault);

        if (await _unitOfWork.Customers.GetAddressAsync(addressId, cancellationToken) is not { } address)
            return Result.Failure(CustomerErrors.Address.NotFound);

        await _unitOfWork.Customers.BulkRemoveIsDefaultAddressAsync(customer, cancellationToken);

        address.IsDefault = true;
        customer.DefaultAddressId = addressId;
        customer.DefaultAddress = address;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> UpdateAddressAsync(int addressId, UpdateAddressRequest request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Customers.GetAddressAsync(addressId, cancellationToken) is not { } address)
            return Result.Failure(CustomerErrors.Address.NotFound);

        request.Adapt(address);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAddressAsync(string customerId, int addressId, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Customers.GetAsync(customerId, cancellationToken) is not { } customer)
            return Result.Failure(CustomerErrors.NotFound);

        if (await _unitOfWork.Customers.GetAddressAsync(addressId, cancellationToken) is not { } address)
            return Result.Failure(CustomerErrors.Address.NotFound);

        if (address.IsDefault)
        {
            customer.DefaultAddressId = null;
            customer.DefaultAddress = null;
        }

        _unitOfWork.Customers.Delete(address);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    #endregion

    #region PhoneNumber



    #endregion




}
