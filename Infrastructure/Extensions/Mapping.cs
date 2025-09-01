﻿namespace Infrastructure.Extensions;

internal static class Mapping
{
    internal static Result ToDomain(this IdentityResult result)
    {
        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    internal static ApplicationUser CreateIdentity(this User user)
        => new()
        {
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsDisabled = user.IsDisabled
        };

    internal static ApplicationRole CreateIdentity(this Role role)
        => new()
        {
            Name = role.Name,
            IsDisabled = role.IsDisabled,
            IsDefault = role.IsDefault
        };
}
