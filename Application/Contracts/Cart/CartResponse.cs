namespace Application.Contracts.Cart;

public record CartResponse(
    IEnumerable<CartProductResponse> Products,
    decimal TotalPrice
);
