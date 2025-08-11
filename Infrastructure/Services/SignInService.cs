namespace Infrastructure.Services;

public class SignInService(SignInManager<ApplicationUser> signInManager) : ISignInService
{
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

    public async Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure)
        => await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
}
