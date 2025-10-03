namespace Application.Contracts.User;

public record ChangeUserNameRequest(
    string NewUserName
);

#region Validation

public class ChangeUserNameRequestValidator : AbstractValidator<ChangeUserNameRequest>
{
    public ChangeUserNameRequestValidator()
    {
        RuleFor(x => x.NewUserName)
            .NotEmpty()
            .Matches(RegexPatterns.AlphanumericUnderscorePattern)
            .Length(3, 20);
    }
}

#endregion