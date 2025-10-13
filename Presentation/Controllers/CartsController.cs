using Application.Contracts.Cart;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = DefaultRoles.Customer.Name)]
[EnableRateLimiting(RateLimitingOptions.PolicyNames.UserLimit)]
public class CartsController(ICartService cartService) : ControllerBase
{
    private readonly ICartService _cartService = cartService;

    [HttpGet("")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await _cartService.GetAsync(User.GetId()!, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetCount(CancellationToken cancellationToken)
    {
        var result = await _cartService.CountAsync(User.GetId()!, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPost("{productId}")]
    public async Task<IActionResult> Add([FromRoute] int productId, [FromBody] CartQuantityRequest request, CancellationToken cancellationToken)
    {
        var result = await _cartService.AddAsync(User.GetId()!, productId, request.Quantity, cancellationToken);

        return result.IsSuccess
            ? Created()
            : result.ToProblem();
    }

    [HttpPut("{productId}")]
    public async Task<IActionResult> Delete([FromRoute] int productId, [FromBody] CartQuantityRequest request, CancellationToken cancellationToken)
    {
        var result = await _cartService.DeleteAsync(User.GetId()!, productId, request.Quantity, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> Delete([FromRoute] int productId, CancellationToken cancellationToken)
    {
        var result = await _cartService.DeleteAsync(User.GetId()!, productId, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpDelete("")]
    public async Task<IActionResult> Clear(CancellationToken cancellationToken)
    {
        var result = await _cartService.ClearAsync(User.GetId()!, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
}
