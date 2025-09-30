using Application.Contracts.Order;

namespace Presentation.Controllers;

[Route("api/admin/orders")]
[ApiController]
//[Authorize]
public class AdminOrdersController(IOrderManagementService orderManagementService) : ControllerBase
{
    private readonly IOrderManagementService _orderManagementService = orderManagementService;

    [HttpGet("status")]
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

    [HttpGet("history")]
    public async Task<IActionResult> GetCurrentHistory([FromQuery] RequestFilters filters, CancellationToken cancellationToken)
    {
        var result = await _orderManagementService.GetCurrentHistoryAsync(filters, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
}
