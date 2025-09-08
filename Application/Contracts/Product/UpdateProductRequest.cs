namespace Application.Contracts.Product;

public record UpdateProductRequest(
    string Name,
    string Description
);

#region Validation

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .NotEmpty();
    }
}

#endregion