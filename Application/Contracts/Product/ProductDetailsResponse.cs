namespace Application.Contracts.Product;

public record ProductDetailsResponse(
    int Id,
    string Name,
    string Description,
    decimal CurrentPrice,
    decimal SellingPrice,
    int StorageQuantity,
    bool IsAvailable
);
