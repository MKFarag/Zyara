namespace Domain.Errors;

public static class OrderErrors
{
    public static readonly Error EmptyCart =
        new("Order.EmptyCart", "The cart is empty", StatusCodes.NotFound);
}
