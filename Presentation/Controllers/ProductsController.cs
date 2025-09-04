namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] RequestFilters filters, CancellationToken cancellationToken)
        => Ok(await _productService.GetAllAsync(filters, cancellationToken));
}
