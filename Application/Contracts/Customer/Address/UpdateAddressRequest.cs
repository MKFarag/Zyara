namespace Application.Contracts.Customer.Address;

public record UpdateAddressRequest(
    string Governorate,
    string City,
    string Street,
    string? Note
);

#region Validation

public class UpdateAddressRequestValidator : AbstractValidator<UpdateAddressRequest>
{
    public UpdateAddressRequestValidator()
    {
        RuleFor(a => a.Governorate)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(a => a.City)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(a => a.Street)
            .NotEmpty()
            .MaximumLength(250);
    }
}

#endregion