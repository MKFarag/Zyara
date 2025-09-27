using Application.Contracts.Order;

namespace Presentation.Controllers;

[Route("api/admin/orders")]
[ApiController]
//[Authorize]
public class AdminOrdersController(IOrderManagementService orderManagementService) : ControllerBase
{
    private readonly IOrderManagementService _orderManagementService = orderManagementService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromBody] OrderStatusRequest request, CancellationToken cancellationToken)
        => Ok(await _orderManagementService.GetAllByStatusAsync(request, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _orderManagementService.GetAsync(id, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
}
