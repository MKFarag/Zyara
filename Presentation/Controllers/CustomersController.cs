using Application.Contracts.Customer.Address;
using Application.Contracts.Customer.PhoneNumber;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    private readonly ICustomerService _customerService = customerService;

    #region Address

    [HttpGet("address")]
	public async Task<IActionResult> GetAllAddress(CancellationToken cancellationToken)
	{
		var result = await _customerService.GetAllAddressesAsync(User.GetId()!, cancellationToken);

		return result.IsSuccess
			? Ok(result.Value)
			: result.ToProblem();
	}

    [HttpGet("default-address")]
	public async Task<IActionResult> GetDefaultAddress(CancellationToken cancellationToken)
	{
		var result = await _customerService.GetDefaultAddressAsync(User.GetId()!, cancellationToken);

		return result.IsSuccess
			? Ok(result.Value)
			: result.ToProblem();
	}

    [HttpPut("default-address/{addressId}")]
	public async Task<IActionResult> SetDefaultAddress([FromRoute] int addressId, CancellationToken cancellationToken)
	{
		var result = await _customerService.SetDefaultAddressAsync(User.GetId()!, addressId, cancellationToken);

		return result.IsSuccess
			? NoContent()
			: result.ToProblem();
	}

    [HttpPost("address")]
	public async Task<IActionResult> AddAddress([FromBody] AddressRequest request, CancellationToken cancellationToken)
	{
		var result = await _customerService.AddAddressAsync(User.GetId()!, request, cancellationToken);

		return result.IsSuccess
			? CreatedAtAction(nameof(GetAllAddress), result.Value)
            : result.ToProblem();
	}

    [HttpPut("address/{addressId}")]
	public async Task<IActionResult> UpdateAddress([FromRoute] int addressId, [FromBody] UpdateAddressRequest request, CancellationToken cancellationToken)
	{
		var result = await _customerService.UpdateAddressAsync(User.GetId()!, addressId, request, cancellationToken);

		return result.IsSuccess
			? NoContent()
			: result.ToProblem();
	}

    [HttpDelete("address/{addressId}")]
	public async Task<IActionResult> DeleteAddress([FromRoute] int addressId, CancellationToken cancellationToken)
	{
		var result = await _customerService.DeleteAddressAsync(User.GetId()!, addressId, cancellationToken);

		return result.IsSuccess
			? NoContent()
			: result.ToProblem();
	}

	#endregion

	#region PhoneNumber

	[HttpGet("phone")]
	public async Task<IActionResult> GetAllPhoneNumbers(CancellationToken cancellationToken)
	{
		var result = await _customerService.GetAllPhoneNumbersAsync(User.GetId()!, cancellationToken);

		return result.IsSuccess
			? Ok(result.Value)
			: result.ToProblem();
	}

	[HttpGet("primary-phone")]
	public async Task<IActionResult> GetPrimaryPhoneNumber(CancellationToken cancellationToken)
	{
		var result = await _customerService.GetPrimaryPhoneNumber(User.GetId()!, cancellationToken);

		return result.IsSuccess
			? Ok(result.Value)
			: result.ToProblem();
	}

	[HttpPut("primary-phone")]
	public async Task<IActionResult> SetPrimaryPhoneNumber([FromBody] PhoneNumberRequest request, CancellationToken cancellationToken)
	{
		var result = await _customerService.SetPrimaryPhoneNumberAsync(User.GetId()!, request.PhoneNumber, cancellationToken);

		return result.IsSuccess
			? NoContent()
			: result.ToProblem();
	}

	[HttpPost("phone")]
	public async Task<IActionResult> AddPhoneNumber([FromBody] PhoneNumberRequest request, CancellationToken cancellationToken)
	{
		var result = await _customerService.AddPhoneNumberAsync(User.GetId()!, request.PhoneNumber, cancellationToken);

		return result.IsSuccess
			? CreatedAtAction(nameof(GetAllPhoneNumbers), result.Value)
			: result.ToProblem();
	}

	[HttpDelete("phone")]
	public async Task<IActionResult> DeletePhoneNumber([FromBody] PhoneNumberRequest request, CancellationToken cancellationToken)
	{
		var result = await _customerService.DeletePhoneNumberAsync(User.GetId()!, request.PhoneNumber, cancellationToken);

		return result.IsSuccess
			? NoContent()
			: result.ToProblem();
	}

	#endregion
}
