namespace Application.Interfaces.Application;

public interface IProductService
{
    Task<IPaginatedList<ProductResponse>> GetAllAsync(RequestFilters filters, bool includeNotAvailable, CancellationToken cancellationToken = default);
    Task<Result<ProductDetailsResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<ProductResponse>> AddAsync(ProductRequest request, CancellationToken cancellationToken = default);
    Task<Result> AddImageAsync(int id, IFormFile image, CancellationToken cancellationToken = default);
    Task<Result> AddImagesAsync(int id, IEnumerable<IFormFile> images, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, UpdateProductRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateCurrentPriceAsync(int id, decimal price, CancellationToken cancellationToken = default);
    Task<Result> UpdateSellingPriceAsync(int id, decimal price, CancellationToken cancellationToken = default);
    Task<Result> IncreaseStorageAsync(int id, int quantity, CancellationToken cancellationToken = default);
    Task<Result> DecreaseStorageAsync(int id, int quantity, CancellationToken cancellationToken = default);
}
