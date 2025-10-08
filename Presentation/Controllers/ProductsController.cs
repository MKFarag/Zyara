using Application.Contracts.Files;
using Application.Contracts.Product;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
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

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] ProductRequest request, CancellationToken cancellationToken)
    {
        var result = await _productService.AddAsync(request, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(Get), new { result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPost("{id}/image")]
    public async Task<IActionResult> AddImage([FromRoute] int id, [FromForm] UploadImageRequest request, CancellationToken cancellationToken)
    {
        var result = await _productService.AddImageAsync(id, request.Image, cancellationToken);

        return result.IsSuccess
            ? Created()
            : result.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest();

        var result = await _productService.UpdateAsync(id, request, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpPut("{id}/current-price")]
    public async Task<IActionResult> UpdateCurrentPrice([FromRoute] int id, [FromBody] UpdateProductPriceRequest request, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest();

        var result = await _productService.UpdateCurrentPriceAsync(id, request.Price, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpPut("{id}/selling-price")]
    public async Task<IActionResult> UpdateSellingPrice([FromRoute] int id, [FromBody] UpdateProductPriceRequest request, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest();

        var result = await _productService.UpdateSellingPriceAsync(id, request.Price, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpPut("{id}/increase")]
    public async Task<IActionResult> IncreaseQuantity([FromRoute] int id, [FromBody] UpdateProductQuantityRequest request, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest();

        var result = await _productService.IncreaseStorageAsync(id, request.Quantity, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpPut("{id}/decrease")]
    public async Task<IActionResult> DecreaseQuantity([FromRoute] int id, [FromBody] UpdateProductQuantityRequest request, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest();

        var result = await _productService.DecreaseStorageAsync(id, request.Quantity, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
}
