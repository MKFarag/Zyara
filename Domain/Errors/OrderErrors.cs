namespace Domain.Errors;

public static class OrderErrors
{
    public static readonly Error NotFound =
        new("Order.NotFound", "No order found", StatusCodes.NotFound);

    public static readonly Error EmptyCart =
        new("Order.EmptyCart", "The cart is empty", StatusCodes.NotFound);

    public static readonly Error AccessDenied =
        new("Order.AccessDenied", "This order is for another customer", StatusCodes.Forbidden);

    public static readonly Error CannotBeCancelled =
        new("Order.CannotBeCancelled", "This order can not be cancelled", StatusCodes.Forbidden);
}
