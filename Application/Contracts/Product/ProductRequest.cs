namespace Application.Contracts.Product;

public record ProductRequest(
    string Name,
    string Description,
    int StorageQuantity,
    decimal SellingPrice
);

#region Validation

public class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.StorageQuantity)
            .NotEmpty()
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.SellingPrice)
            .NotEmpty()
            .GreaterThanOrEqualTo(0);
    }
}

#endregion