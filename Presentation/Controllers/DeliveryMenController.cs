using Application.Contracts.DeliveryMan;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeliveryMenController(IDeliveryManService deliveryManService) : ControllerBase
{
    private readonly IDeliveryManService _deliveryManService = deliveryManService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDisabled, CancellationToken cancellationToken)
        => Ok(await _deliveryManService.GetAllAsync(includeDisabled, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _deliveryManService.GetAsync(id, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] DeliveryManRequest request, CancellationToken cancellationToken)
    {
        var result = await _deliveryManService.AddAsync(request, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(Get), new { result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] DeliveryManRequest request, CancellationToken cancellationToken)
    {
        var result = await _deliveryManService.UpdateAsync(id, request, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _deliveryManService.ToggleStatusAsync(id, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
}
