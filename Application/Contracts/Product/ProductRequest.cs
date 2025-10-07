namespace Application.Contracts.Product;

public record ProductRequest(
    string Name,
    string Description,
    int StorageQuantity,
    decimal SellingPrice,
    IEnumerable<IFormFile> Images
);

#region Validation

public class ProductRequestValidator : AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        RuleForEach(x => x.Images)
            .SetValidator(new FileSizeValidator())
            .SetValidator(new BlockedSignaturesValidator());

        RuleFor(x => x.Images)
            .Must((request, context) =>
            {
                foreach (var img in request.Images)
                {
                    var extension = Path.GetExtension(img.FileName.ToLower());

                    if (!FileSettings.AllowedImagesExtensions.Contains(extension))
                        return false;
                }
                return true;
            })
            .WithMessage("Image extension is not allowed")
            .When(x => x.Images is not null);

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