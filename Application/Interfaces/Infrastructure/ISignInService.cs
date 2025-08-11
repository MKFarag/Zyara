using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Infrastructure;

public interface ISignInService
{
    Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure);
}
