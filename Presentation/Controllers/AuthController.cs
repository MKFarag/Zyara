namespace Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.GetTokenAsync(request.Identifier, request.Password, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken);

        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(request, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPost("resend-confirmation")]
    public async Task<IActionResult> ResendConfirmation([FromBody] ResendConfirmationEmailRequest request)
    {
        var result = await _authService.ResendConfirmationEmailAsync(request.Email);

        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }

    [HttpPost("confirm")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
    {
        var result = await _authService.ConfirmEmailAsync(request.UserId, request.Code);

        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
    {
        var result = await _authService.SendResetPasswordCodeAsync(request.Email);

        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authService.ResetPasswordAsync(request);

        return result.IsSuccess
    ? Ok()
    : result.ToProblem();
    }
}
