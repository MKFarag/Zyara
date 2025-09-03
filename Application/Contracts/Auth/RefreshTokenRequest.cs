namespace Application.Contracts.Auth;

public record RefreshTokenRequest(
    string Token,
    string RefreshToken
);

#region Validator

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();

        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}

#endregion