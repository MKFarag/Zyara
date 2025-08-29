namespace Domain.Entities;

public sealed class Cart
{
    public string CustomerId { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    public Customer Customer { get; set; } = default!;
    public Product Product { get; set; } = default!;
}
