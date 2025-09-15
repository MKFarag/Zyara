namespace Domain.Errors;

public record CustomerErrors
{
    public partial record Address
    {
        public static readonly Error NotFound =
            new("Address.NotFound", "No address found", StatusCodes.NotFound);

        public static readonly Error AlreadyDefault =
            new("Address.AlreadyDefault", "This address is default already", StatusCodes.BadRequest);

        public static readonly Error DeleteDefault =
            new("Address.DeleteDefault", "You can not delete a default address", StatusCodes.BadRequest);
    }
    public partial record PhoneNumber
    {
        public static readonly Error NotFound =
            new("PhoneNumber.NotFound", "No phone number found", StatusCodes.NotFound);

        public static readonly Error DeletePrimary =
            new("PhoneNumber.DeletePrimary", "You can not delete a primary phone number", StatusCodes.BadRequest);

        public static readonly Error AlreadyPrimary =
            new("PhoneNumber.AlreadyPrimary", "This phone number is primary already", StatusCodes.BadRequest);

        public static readonly Error Duplicated =
            new("PhoneNumber.Duplicated", "You added this phone number before", StatusCodes.Conflict);
    }

    public static readonly Error NotFound =
        new("Customer.NotFound", "No customer found", StatusCodes.NotFound);
}
