namespace Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int StorageQuantity { get; set; }
    public float CurrentPrice { get; set; }
    public float SellingPrice { get; set; }

    // TODO: Add ProductImage table and its navigation property
}
