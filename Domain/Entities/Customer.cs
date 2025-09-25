namespace Domain.Entities;

public sealed class Customer
{
    public string Id { get; set; } = string.Empty;

    public ICollection<Address> Addresses { get; set; } = [];
    public ICollection<Cart> CartItems { get; set; } = [];
    public ICollection<Order> Orders { get; set; } = [];
    public ICollection<CustomerPhoneNumber> PhoneNumbers { get; set; } = []; 
}
