namespace Application.Contracts.Product;

public record ProductResponse(
    int Id,
    string Name,
    string Description,
    decimal CurrentPrice,
    bool IsAvailable,
    string MainImageUrl
);
