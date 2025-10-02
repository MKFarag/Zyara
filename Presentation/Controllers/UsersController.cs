namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class UsersController(IUserManagementService userManagementService) : ControllerBase
{
    private readonly IUserManagementService _userManagementService = userManagementService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        => Ok(await _userManagementService.GetAllAsync(cancellationToken));

}
