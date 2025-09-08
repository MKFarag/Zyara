namespace Application.Contracts.Product;

public record UpdateProductQuantityRequest(
    int Quantity
);

#region Validation

public class UpdateProductQuantityRequestValidator : AbstractValidator<UpdateProductQuantityRequest>
{
    public UpdateProductQuantityRequestValidator()
    {
        RuleFor(x => x.Quantity)
            .NotEmpty()
            .GreaterThanOrEqualTo(1);
    }
}

#endregion