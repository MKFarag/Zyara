namespace Domain.Entities;

public class CustomerPhoneNumber
{
    public string CustomerId { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }

    public virtual Customer Customer { get; set; } = default!;
}
