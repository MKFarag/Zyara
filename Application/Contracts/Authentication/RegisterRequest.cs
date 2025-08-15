namespace Application.Contracts.Authentication;

public record RegisterRequest(
    string Email,
    string UserName,
    string Password,
    string FirstName,
    string LastName
);

#region Validation

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.UserName)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password should be at least 8 characters and should contains digit, Lowercase, Uppercase and NonAlphanumeric");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3, 50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3, 50);
    }
}

#endregion