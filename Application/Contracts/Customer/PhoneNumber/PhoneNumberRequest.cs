namespace Application.Contracts.Customer.PhoneNumber;

public record PhoneNumberRequest(
    string PhoneNumber
);

#region Validation

public class PhoneNumberRequestValidator : AbstractValidator<PhoneNumberRequest>
{
    public PhoneNumberRequestValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Length(11);
    }
}

#endregion