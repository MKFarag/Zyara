using Application.Contracts.DeliveryMan;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableRateLimiting(RateLimitingOptions.PolicyNames.Fixed)]
public class DeliveryMenController(IDeliveryManService deliveryManService) : ControllerBase
{
    private readonly IDeliveryManService _deliveryManService = deliveryManService;

    [HttpGet("")]
    [HasPermission(Permissions.GetDeliveryMen)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDisabled, CancellationToken cancellationToken)
        => Ok(await _deliveryManService.GetAllAsync(includeDisabled, cancellationToken));

    [HttpGet("{id}")]
    [HasPermission(Permissions.GetDeliveryMen)]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _deliveryManService.GetAsync(id, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpGet("{id}/orders")]
    [HasPermission(Permissions.GetDeliveryMenOrders)]
    public async Task<IActionResult> GetOrders([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _deliveryManService.GetOrdersAsync(id, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPost("")]
    [HasPermission(Permissions.AddDeliveryMen)]
    public async Task<IActionResult> Add([FromBody] DeliveryManRequest request, CancellationToken cancellationToken)
    {
        var result = await _deliveryManService.AddAsync(request, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(Get), new { result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdateDeliveryMen)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] DeliveryManRequest request, CancellationToken cancellationToken)
    {
        var result = await _deliveryManService.UpdateAsync(id, request, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpPut("{deliveryManId}/set-order/{orderId}")]
    [HasPermission(Permissions.SetDeliveryMenOrders)]
    public async Task<IActionResult> SetToOrder([FromRoute] int deliveryManId, [FromRoute] int orderId, CancellationToken cancellationToken)
    {
        var result = await _deliveryManService.SetToOrderAsync(deliveryManId, orderId, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpPut("{id}/toggle-status")]
    [HasPermission(Permissions.UpdateDeliveryMenStatus)]
    public async Task<IActionResult> ToggleStatus([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _deliveryManService.ToggleStatusAsync(id, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
}
