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
            .SetValidator(new BlockedSignaturesValidator());

        RuleFor(x => x.Image)
            .Must((request, context) =>
            {
                var extension = Path.GetExtension(request.Image.FileName.ToLower());
                return FileSettings.AllowedImagesExtensions.Contains(extension);
            })
            .WithMessage("Image extension is not allowed")
            .When(x => x.Image is not null);
    }
}

#endregion
