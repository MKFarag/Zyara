namespace Application.Contracts.Cart;

public record CartQuantityRequest(
    int Quantity
);

#region Validation

public class CartQuantityRequestValidator : AbstractValidator<CartQuantityRequest>
{
    public CartQuantityRequestValidator()
    {
        RuleFor(x => x.Quantity)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}

#endregion