using Application.Contracts.User;

namespace Presentation.Controllers;

[Route("me")]
[ApiController]
[Authorize]
[EnableRateLimiting(RateLimitingOptions.PolicyNames.UserLimit)]
public class AccountController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("")]
    public async Task<IActionResult> Info(CancellationToken cancellationToken)
    {
        var result = await _userService.GetProfileAsync(User.GetId()!, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPut("")]
    public async Task<IActionResult> Info([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.UpdateProfileAsync(User.GetId()!, request, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.ChangePasswordAsync(User.GetId()!, request, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpPost("change-email")]
    public async Task<IActionResult> ChangeEmailRequest(ChangeEmailRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.ChangeEmailRequestAsync(User.GetId()!, request.NewEmail, cancellationToken);

        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }

    [HttpPut("change-email")]
    public async Task<IActionResult> ConfirmChangeEmail([FromBody] ConfirmChangeEmailRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.ConfirmChangeEmailAsync(User.GetId()!, request, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpPut("change-username")]
    public async Task<IActionResult> ChangeUserName([FromBody] ChangeUserNameRequest request, CancellationToken cancellationToken)
    {
        var result = await _userService.ChangeUserNameAsync(User.GetId()!, request.NewUserName, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
}
