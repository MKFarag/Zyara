namespace Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public float TotalAmount { get; set; }
    public int DeliveryManId { get; set; }
    public string Status { get; set; } = default!;
    public int ShippingAddressId { get; set; }

    public virtual Customer Customer { get; set; } = default!;
    public virtual DeliveryMan DeliveryMan { get; set; } = default!;
    public virtual Address ShippingAddress { get; set; } = default!;
}
