namespace Application.Contracts.Files;

public record UploadImagesRequest(
    IEnumerable<IFormFile> Images
);

#region Validation

public class UploadImagesRequestValidator : AbstractValidator<UploadImagesRequest>
{
    public UploadImagesRequestValidator()
    {
        RuleForEach(x => x.Images)
            .SetValidator(new FileSizeValidator())
            .SetValidator(new BlockedSignaturesValidator())
            .SetValidator(new FileNameValidator())
            .SetValidator(new ImageExtensionValidator());
    }
}

#endregion