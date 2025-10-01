namespace Application.Contracts.User;

public record UpdateProfileRequest(
    string FirstName,
    string LastName
);

#region Validation

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50);
    }
}

#endregion