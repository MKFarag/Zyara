namespace Application.Contracts.Order;

public record OrderDateRequest(
    DateOnly Date
);

#region Validation

public class OrderDateRequestValidator : AbstractValidator<OrderDateRequest>
{
    public OrderDateRequestValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty();
    }
}

#endregion