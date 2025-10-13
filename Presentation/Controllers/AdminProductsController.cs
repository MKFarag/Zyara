using Application.Contracts.Files;
using Application.Contracts.Product;

namespace Presentation.Controllers;

[Route("api/admin/products")]
[ApiController]
[Authorize]
[EnableRateLimiting(RateLimitingOptions.PolicyNames.Fixed)]
public class AdminProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpPost("")]
    [HasPermission(Permissions.AddProducts)]
    public async Task<IActionResult> Add([FromBody] ProductRequest request, CancellationToken cancellationToken)
    {
        var result = await _productService.AddAsync(request, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(ProductsController.Get), new { result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPost("images")]
    [HasPermission(Permissions.AddProducts)]
    public async Task<IActionResult> Add(
        [FromBody] ProductRequest productRequest, [FromForm] UploadImagesRequest imagesRequest, 
        [FromServices] IValidator<UploadImagesRequest> validator, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(imagesRequest, cancellationToken);

        if (!validationResult.IsValid)
            return this.ToProblem(validationResult);

        var result = await _productService.AddAsync(productRequest, imagesRequest.Images, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(ProductsController.Get), new { result.Value.Id }, result.Value)
            : result.ToProblem();
    }

    [HttpPost("{id}/images")]
    [HasPermission(Permissions.UpdateProducts)]
    public async Task<IActionResult> AddImages([FromRoute] int id, [FromForm] UploadImagesRequest request, [FromServices] IValidator<UploadImagesRequest> validator, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return this.ToProblem(validationResult);

        var result = await _productService.AddImagesAsync(id, request.Images, cancellationToken);

        return result.IsSuccess
            ? Created()
            : result.ToProblem();
    }

    [HttpPut("{productId}/images/{imageId}")]
    [HasPermission(Permissions.UpdateProducts)]
    public async Task<IActionResult> SetMain([FromRoute] int productId, [FromRoute] int imageId, CancellationToken cancellationToken)
    {
        var result = await _productService.SetMainImageAsync(productId, imageId, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpDelete("{productId}/images/{imageId}")]
    [HasPermission(Permissions.UpdateProducts)]
    public async Task<IActionResult> DeleteImage([FromRoute] int productId, [FromRoute] int imageId, CancellationToken cancellationToken)
    {
        var result = await _productService.DeleteImageAsync(productId, imageId, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdateProducts)]
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
    [HasPermission(Permissions.UpdateProductsPrice)]
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
    [HasPermission(Permissions.UpdateProductsDiscount)]
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
    [HasPermission(Permissions.UpdateProductsQuantity)]
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
    [HasPermission(Permissions.UpdateProductsQuantity)]
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
