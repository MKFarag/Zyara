namespace Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public float UnitPrice { get; set; }
    public float TotalPrice => Quantity * UnitPrice;

    public virtual Order Order { get; set; } = default!;
    public virtual Product Product { get; set; } = default!;
}
