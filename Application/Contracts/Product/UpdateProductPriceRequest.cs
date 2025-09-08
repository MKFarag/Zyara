namespace Application.Contracts.Product;

public record UpdateProductPriceRequest(
    decimal Price
);

#region Validation

public class UpdateProductPriceRequestValidator : AbstractValidator<UpdateProductPriceRequest>
{
    public UpdateProductPriceRequestValidator()
    {
        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThanOrEqualTo(0);
    }
}

#endregion