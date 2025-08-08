namespace Domain.Entities;

public class Cart
{
    public string CustomerId { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    public virtual Customer Customer { get; set; } = default!;
    public virtual Product Product { get; set; } = default!;
}
