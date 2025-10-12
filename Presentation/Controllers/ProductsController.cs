namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting(RateLimitingOptions.PolicyNames.Fixed)]
public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] RequestFilters filters, [FromQuery] bool includeNotAvailable, CancellationToken cancellationToken)
        => Ok(await _productService.GetAllAsync(filters, includeNotAvailable, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest();

        var result = await _productService.GetAsync(id, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
}
