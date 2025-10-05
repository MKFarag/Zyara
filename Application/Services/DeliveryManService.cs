namespace Application.Services;

public class DeliveryManService(IUnitOfWork unitOfWork) : IDeliveryManService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<DeliveryManResponse>> GetAllAsync(bool includeDisabled, CancellationToken cancellationToken = default)
        => await _unitOfWork.DeliveryMen
            .FindAllProjectionAsync<DeliveryManResponse>
            (
                d => includeDisabled || !d.IsDisabled,
                cancellationToken
            );

    public async Task<Result<DeliveryManDetailResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var deliveryMan = await _unitOfWork.DeliveryMen
            .FindAsync
            (
                d => d.Id == id,
                [nameof(DeliveryMan.Orders)],
                cancellationToken
            );

        if (deliveryMan is null)
            return Result.Failure<DeliveryManDetailResponse>(DeliveryManErrors.NotFound);

        var response = deliveryMan.Adapt<DeliveryManDetailResponse>();

        return Result.Success(response);
    }

    public async Task<Result<DeliveryManResponse>> AddAsync(DeliveryManRequest request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.DeliveryMen.AnyAsync(d => d.PhoneNumber == request.PhoneNumber, cancellationToken))
            return Result.Failure<DeliveryManResponse>(DeliveryManErrors.DuplicatedPhoneNumber);

        var deliveryMan = request.Adapt<DeliveryMan>();

        await _unitOfWork.DeliveryMen.AddAsync(deliveryMan, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        var response = deliveryMan.Adapt<DeliveryManResponse>();

        return Result.Success(response);
    }

    public async Task<Result> UpdateAsync(int id, DeliveryManRequest request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.DeliveryMen.AnyAsync(d => d.PhoneNumber == request.PhoneNumber && d.Id != id, cancellationToken))
            return Result.Failure(DeliveryManErrors.DuplicatedPhoneNumber);

        if (await _unitOfWork.DeliveryMen.GetAsync(id, cancellationToken) is not { } deliveryMan)
            return Result.Failure(DeliveryManErrors.NotFound);

        deliveryMan = request.Adapt(deliveryMan);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> ToggleStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.DeliveryMen.GetAsync(id, cancellationToken) is not { } deliveryMan)
            return Result.Failure(DeliveryManErrors.NotFound);

        deliveryMan.IsDisabled = !deliveryMan.IsDisabled;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }
}
