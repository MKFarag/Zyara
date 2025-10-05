﻿namespace Application.Contracts.Role;

public record RoleRequest(
    string Name,
    IList<string> Permissions
);

#region Validation

public class RoleRequestValidator : AbstractValidator<RoleRequest>
{
    public RoleRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Permissions)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Permissions)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("You cannot add duplicated permissions for the same role.")
            .When(x => x.Permissions != null);
    }
}

#endregion