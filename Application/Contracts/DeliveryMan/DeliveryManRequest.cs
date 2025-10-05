namespace Application.Contracts.DeliveryMan;

public record DeliveryManRequest(
    string Name,
    string PhoneNumber
);

#region Validation

public class DeliveryManRequestValidator : AbstractValidator<DeliveryManRequest>
{
    public DeliveryManRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Length(11)
            .Matches(RegexPatterns.OnlyNumbers)
            .WithMessage("PhoneNumber must have only number");
    }
}

#endregion