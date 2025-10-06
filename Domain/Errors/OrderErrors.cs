namespace Domain.Errors;

public static class OrderErrors
{
    public static readonly Error NotFound =
        new("Order.NotFound", "No order found", StatusCodes.NotFound);

    public static readonly Error EmptyCart =
        new("Order.EmptyCart", "The cart is empty", StatusCodes.NotFound);

    public static readonly Error InvalidStatus =
        new("Order.InvalidStatus", "Invalid status", StatusCodes.BadRequest);

    public static readonly Error InvalidInput =
        new("Order.InvalidInput", "Invalid input", StatusCodes.BadRequest);

    public static readonly Error AlreadyCompleted =
        new("Order.AlreadyCompleted ", "The order has already been completed (delivered or canceled) and cannot be assigned to a delivery man", StatusCodes.BadRequest);

    public static readonly Error AccessDenied =
        new("Order.AccessDenied", "This order is for another customer", StatusCodes.Forbidden);

    public static readonly Error CannotBeCancelled =
        new("Order.CannotBeCancelled", "This order can not be cancelled", StatusCodes.Forbidden);
}
