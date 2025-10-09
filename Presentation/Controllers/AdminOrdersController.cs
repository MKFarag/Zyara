using Application.Contracts.Order;

namespace Presentation.Controllers;

[Route("api/admin/orders")]
[ApiController]
//[Authorize]
public class AdminOrdersController(IOrderManagementService orderManagementService) : ControllerBase
{
    private readonly IOrderManagementService _orderManagementService = orderManagementService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] RequestFilters filters, [FromBody] OrderStatusRequest request, CancellationToken cancellationToken)
        => Ok(await _orderManagementService.GetAllByStatusAsync(filters, request, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _orderManagementService.GetAsync(id, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpGet("current-history")]
    public async Task<IActionResult> GetCurrentHistory([FromQuery] RequestFilters filters, CancellationToken cancellationToken)
        => Ok(await _orderManagementService.GetCurrentHistoryAsync(filters, cancellationToken));

    [HttpGet("date-history")]
    public async Task<IActionResult> GetHistoryByDate([FromBody] OrderDateRequest request, [FromQuery] RequestFilters filters, CancellationToken cancellationToken)
        => Ok(await _orderManagementService.GetHistoryByDateAsync(filters, request.Date, cancellationToken));

    [HttpGet("month-history")]
    public async Task<IActionResult> GetHistoryByMonth([FromQuery] int month, [FromQuery] RequestFilters filters, CancellationToken cancellationToken)
    {
        if (month is < 1 or > 12)
            return BadRequest("Month must be between 1 and 12.");

        return Ok(await _orderManagementService.GetHistoryByMonthAsync(filters, month, cancellationToken));
    }

    [HttpGet("year-history")]
    public async Task<IActionResult> GetHistoryByYear([FromQuery] int year, [FromQuery] RequestFilters filters, CancellationToken cancellationToken)
    {
        if (year < 2025 || year > DateTime.UtcNow.Year)
            return BadRequest($"Year must be between 2025 and {DateTime.UtcNow.Year}.");

        return Ok(await _orderManagementService.GetHistoryByYearAsync(filters, year, cancellationToken));
    }

    [HttpGet("current-earning")]
    public async Task<IActionResult> GetCurrentEarning(CancellationToken cancellationToken)
    {
        var result = await _orderManagementService.GetCurrentEarningAsync(cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpGet("date-earning")]
    public async Task<IActionResult> GetEarningByDate([FromBody] OrderDateRequest request, CancellationToken cancellationToken)
    {
        var result = await _orderManagementService.GetEarningByDateAsync(request.Date, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpGet("month-earning")]
    public async Task<IActionResult> GetEarningByMonth([FromQuery] int month, CancellationToken cancellationToken)
    {
        if (month is < 1 or > 12)
            return BadRequest("Month must be between 1 and 12.");

        var result = await _orderManagementService.GetEarningByMonthAsync(month, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpGet("year-earning")]
    public async Task<IActionResult> GetEarningByYear([FromQuery] int year, CancellationToken cancellationToken)
    {
        if (year < 2025 || year > DateTime.UtcNow.Year)
            return BadRequest($"Year must be between 2025 and {DateTime.UtcNow.Year}.");

        var result = await _orderManagementService.GetEarningByYearAsync(year, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPut("{id}/change-status")]
    public async Task<IActionResult> ChangeStatus([FromRoute] int id, [FromBody] OrderStatusRequest request, CancellationToken cancellationToken)
    {
        var result = await _orderManagementService.ChangeStatusAsync(id, request, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
}
