namespace Domain.Entities;

public class Customer
{
    public string Id { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public int DefaultAddressId { get; set; }

    public virtual ApplicationUser User { get; set; } = default!;
    public virtual Address DefaultAddress { get; set; } = default!;
    public virtual List<CustomerPhoneNumber> PhoneNumbers { get; set; } = []; 
}
