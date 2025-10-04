namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] int year, CancellationToken cancellationToken)
    {
        if (year < 0)
            return BadRequest("Year must be a positive number");

        return Ok(await _orderService.GetAllAsync(User.GetId()!, year, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _orderService.GetAsync(User.GetId()!, id, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpGet("{id}/track")]
    public async Task<IActionResult> Track([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _orderService.TrackAsync(User.GetId()!, id, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPost("")]
    public async Task<IActionResult> PlaceOrder(CancellationToken cancellationToken)
    {
        var result = await _orderService.PlaceOrderAsync(User.GetId()!, cancellationToken);

        return result.IsSuccess
            ? Created()
            : result.ToProblem();
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelOrder([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _orderService.CancelAsync(User.GetId()!, id, cancellationToken);

        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }
}
