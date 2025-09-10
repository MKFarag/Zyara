namespace Application.Contracts.Customer.Address;

public record AddressRequest(
    string Governorate,
    string City,
    string Street,
    string? Note,
    bool IsDefault
);

#region Validation

public class AddressRequestValidator : AbstractValidator<AddressRequest>
{
    public AddressRequestValidator()
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

        RuleFor(a => a.IsDefault)
            .NotEmpty();
    }
}

#endregion