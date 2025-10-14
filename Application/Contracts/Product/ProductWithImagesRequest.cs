namespace Application.Contracts.Product;

public record ProductWithImagesRequest(
    string Name,
    string Description,
    int StorageQuantity,
    decimal SellingPrice,
    IEnumerable<IFormFile> Images
);

#region Validation

public class ProductWithImageRequestValidator : AbstractValidator<ProductWithImagesRequest>
{
    public ProductWithImageRequestValidator()
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

        RuleForEach(x => x.Images)
            .SetValidator(new FileSizeValidator())
            .SetValidator(new BlockedSignaturesValidator())
            .SetValidator(new FileNameValidator())
            .SetValidator(new ImageExtensionValidator());
    }
}

#endregion