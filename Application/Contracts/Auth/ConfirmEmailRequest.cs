﻿namespace Application.Contracts.Auth;

public record ConfirmEmailRequest(
    string UserId,
    string Token
);

#region Validator

public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Token)
            .NotEmpty();
    }
}

#endregion