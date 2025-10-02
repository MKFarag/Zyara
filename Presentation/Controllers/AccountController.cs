namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class AccountController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("")]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
        => Ok(await _userService.GetProfileAsync(User.GetId()!, cancellationToken));



}
