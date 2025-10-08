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
    }
}

#endregion