namespace Domain.Errors;

public record ProductErrors
{
    public static readonly Error NotFound =
        new("Product.NotFound", "No product found", StatusCodes.NotFound);

    public static readonly Error NotAvailable =
        new("Product.NotAvailable", "This product is not available", StatusCodes.NotFound);

    public static readonly Error DuplicatedName =
        new("Product.DuplicatedName", "This name is already taken", StatusCodes.Conflict);

    public static readonly Error InvalidPrice =
        new("Product.InvalidPrice", "You can not change the current price to be higher than the selling price", StatusCodes.BadRequest);

    public static readonly Error InvalidQuantity =
        new("Product.InvalidQuantity", "Storage quantity cannot be decreased below available stock", StatusCodes.BadRequest);

    public static readonly Error EmptyStorage =
        new("Product.EmptyStorage", "The storage is already empty", StatusCodes.BadRequest);
}
