namespace Domain.Entities;

public sealed class CustomerPhoneNumber
{
    public string CustomerId { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}
