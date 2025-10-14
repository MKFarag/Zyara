namespace Application.Contracts.Files;

public record UploadImageRequest(
    IFormFile Image
);

#region Validation

public class UploadImageRequestValidator : AbstractValidator<UploadImageRequest>
{
    public UploadImageRequestValidator()
    {
        RuleFor(x => x.Image)
            .SetValidator(new FileSizeValidator())
            .SetValidator(new BlockedSignaturesValidator())
            .SetValidator(new FileNameValidator())
            .SetValidator(new ImageExtensionValidator());
    }
}

#endregion
