namespace Domain.Errors;

public static class DeliveryManErrors
{
    public static readonly Error NotFound =
        new("DeliveryMan.NotFound", "No delivery man found", StatusCodes.NotFound);

    public static readonly Error DuplicatedPhoneNumber =
        new("DeliveryMan.DuplicatedPhoneNumber", "This phone number is already used", StatusCodes.NotFound);

    public static readonly Error Disabled =
        new("DeliveryMan.Disabled", "This delivery man ID is currently disabled", StatusCodes.BadRequest);
}
