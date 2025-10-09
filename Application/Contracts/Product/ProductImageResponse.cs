namespace Application.Contracts.Product;

public record ProductImageResponse(
    int Id,
    string Url,
    bool IsMain
);
