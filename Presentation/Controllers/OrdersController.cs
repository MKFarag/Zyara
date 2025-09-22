using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [HttpPost("")]
    public async Task<IActionResult> PlaceOrder(CancellationToken cancellationToken)
    {
        var result = await _orderService.PlaceOrderAsync("01996cbd-0e0e-73e6-acd9-c7136a54092c", cancellationToken);

        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }
}
