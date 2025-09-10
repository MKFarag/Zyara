namespace Domain.Errors;

public record CustomerErrors
{
    public partial record Address
    {
        public static readonly Error NotFound =
            new("Address.NotFound", "No address found", StatusCodes.NotFound);

        public static readonly Error AlreadyDefault =
            new("Address.AlreadyDefault", "This address is default already", StatusCodes.BadRequest);
    }

    public static readonly Error NotFound =
        new("Customer.NotFound", "No customer found", StatusCodes.NotFound);
}
