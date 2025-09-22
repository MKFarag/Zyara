using Application.Contracts.Order;

namespace Application.Services;

public class OrderService(IUnitOfWork unitOfWork) : IOrderService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    //public async Task<Result> PlaceOrderAsync(string customerId, CancellationToken cancellationToken = default)
    //{
    //    var customer = await _unitOfWork.Customers
    //        .FindAsync
    //        (
    //            c => c.Id == customerId,
    //            [nameof(Customer.PhoneNumbers), nameof(Customer.Addresses)],
    //            cancellationToken
    //        );

    //    if (customer is null)
    //        return Result.Failure(CustomerErrors.NotFound);

    //    if (customer.Addresses.FirstOrDefault(a => a.IsDefault) is not { } defaultAddress)
    //        return Result.Failure(CustomerErrors.Address.NotFound);

    //    if (customer.PhoneNumbers.FirstOrDefault(p => p.IsPrimary) is not { } primaryPhoneNumber)
    //        return Result.Failure(CustomerErrors.PhoneNumber.NotFound);

        

    //}
}
