namespace Domain.Entities;

public sealed class Customer
{
    public string Id { get; set; } = string.Empty;
    public int DefaultAddressId { get; set; }

    public Address DefaultAddress { get; set; } = default!;
    public List<CustomerPhoneNumber> PhoneNumbers { get; set; } = []; 
}
