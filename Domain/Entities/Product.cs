namespace Domain.Entities;

public sealed class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int StorageQuantity { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal SellingPrice { get; set; }

    public bool IsAvailable => StorageQuantity > 0;

    public ICollection<ProductImage> Images { get; set; } = [];
}
