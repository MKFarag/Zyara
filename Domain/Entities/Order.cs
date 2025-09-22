namespace Domain.Entities;

public sealed class Order
{
    public int Id { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal ShippingCost { get; set; }
    public decimal TotalAmount { get; set; }
    public int? DeliveryManId { get; set; }
    public string Status { get; set; } = default!;
    public string ShippingAddress { get; set; } = string.Empty;

    public ICollection<OrderItem> OrderItems { get; set; } = [];
    public Customer Customer { get; set; } = default!;
    public DeliveryMan? DeliveryMan { get; set; } = default!;
}
