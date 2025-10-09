namespace Application.Services;

public class ProductService(IUnitOfWork unitOfWork, IFileStorageService fileStorageService) : IProductService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IFileStorageService _fileStorageService = fileStorageService;

    private static readonly string _allowedSearchColumn = nameof(Product.Name);
    private static readonly HashSet<string> _allowedSortColumns = new(StringComparer.OrdinalIgnoreCase) 
    { nameof(Product.Id), nameof(Product.CurrentPrice) };

    public async Task<IPaginatedList<ProductResponse>> GetAllAsync(RequestFilters filters, bool includeNotAvailable, CancellationToken cancellationToken = default)
    {
        var checkedFilters = filters.Check(_allowedSortColumns);

        IPaginatedList<ProductResponse> response;

        if (includeNotAvailable)
            response = await _unitOfWork.Products
                .GetPaginatedListAsync<ProductResponse>
                (
                    checkedFilters.PageNumber,
                    checkedFilters.PageSize,
                    checkedFilters.SearchValue,
                    _allowedSearchColumn,
                    checkedFilters.SortColumn!,
                    checkedFilters.SortDirection!,
                    ColumnType.String,
                    [nameof(Product.Images)],
                    cancellationToken
                );
        else
            response = await _unitOfWork.Products
                .FindPaginatedListAsync<ProductResponse>
                (
                    p => p.StorageQuantity > 0,
                    checkedFilters.PageNumber,
                    checkedFilters.PageSize,
                    checkedFilters.SearchValue,
                    _allowedSearchColumn,
                    checkedFilters.SortColumn!,
                    checkedFilters.SortDirection!,
                    ColumnType.String,
                    [nameof(Product.Images)],
                    cancellationToken
                );

        return response;
    }

    public async Task<Result<ProductDetailsResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.Products.FindAsync(p => p.Id == id, [nameof(Product.Images)], cancellationToken);

        var response = product.Adapt<ProductDetailsResponse>();

        return Result.Success(response);
    }

    public async Task<Result<ProductResponse>> AddAsync(ProductRequest request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Products.AnyAsync(p => p.Name == request.Name, cancellationToken))
            return Result.Failure<ProductResponse>(ProductErrors.DuplicatedName);

        var product = request.Adapt<Product>();

        await _unitOfWork.Products.AddAsync(product, cancellationToken);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success(product.Adapt<ProductResponse>());
    }

    public async Task<Result> AddImagesAsync(int id, IEnumerable<IFormFile> images, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.Products.TrackedFindAsync
        (
            p => p.Id == id,
            [nameof(Product.Images)],
            cancellationToken
        );

        if (product is null)
            return Result.Failure(ProductErrors.NotFound);

        foreach (var image in images)
        {
            var relativePath = await _fileStorageService.SaveAsync(image, FileTypes.Image, GetName(image), cancellationToken);

            product.Images.Add(new ProductImage
            {
                Url = relativePath,
                IsMain = false
            });
        }

        if (!product.Images.Any(pi => pi.IsMain))
            product.Images.First().IsMain = true;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> SetMainImageAsync(int productId, int imageId, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.Products.TrackedFindAsync
        (
            p => p.Id == productId,
            [nameof(Product.Images)],
            cancellationToken
        );

        if (product is null)
            return Result.Failure(ProductErrors.NotFound);

        if (product.Images.FirstOrDefault(pi => pi.Id == imageId) is not { } image)
            return Result.Failure(ProductErrors.Image.NotFound);

        if (image.IsMain)
            return Result.Failure(ProductErrors.Image.AlreadyMain);

        product.Images.First(pi => pi.IsMain).IsMain = false;

        image.IsMain = true;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteImageAsync(int productId, int imageId, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.Products.TrackedFindAsync
        (
            p => p.Id == productId,
            [nameof(Product.Images)],
            cancellationToken
        );

        if (product is null)
            return Result.Failure(ProductErrors.NotFound);

        if (product.Images.FirstOrDefault(pi => pi.Id == imageId) is not { } image)
            return Result.Failure(ProductErrors.Image.NotFound);

        if (image.IsMain)
            return Result.Failure(ProductErrors.Image.DeleteMain);

        await _fileStorageService.RemoveAsync(image.Url, cancellationToken);

        product.Images.Remove(image);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> UpdateAsync(int id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Products.AnyAsync(p => p.Name == request.Name && p.Id != id, cancellationToken))
            return Result.Failure<ProductResponse>(ProductErrors.DuplicatedName);

        if (await _unitOfWork.Products.GetAsync(id, cancellationToken) is not { } product)
            return Result.Failure(ProductErrors.NotFound);

        product = request.Adapt(product);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> UpdateCurrentPriceAsync(int id, decimal price, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Products.GetAsync(id, cancellationToken) is not { } product)
            return Result.Failure(ProductErrors.NotFound);

        if (product.SellingPrice < price)
            return Result.Failure(ProductErrors.InvalidPrice);

        product.CurrentPrice = price;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> UpdateSellingPriceAsync(int id, decimal price, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Products.GetAsync(id, cancellationToken) is not { } product)
            return Result.Failure(ProductErrors.NotFound);

        product.SellingPrice = price;
        product.CurrentPrice = price;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> IncreaseStorageAsync(int id, int quantity, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Products.GetAsync(id, cancellationToken) is not { } product)
            return Result.Failure(ProductErrors.NotFound);

        product.StorageQuantity += quantity;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DecreaseStorageAsync(int id, int quantity, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Products.GetAsync(id, cancellationToken) is not { } product)
            return Result.Failure(ProductErrors.NotFound);

        if (product.StorageQuantity == 0)
            return Result.Failure(ProductErrors.EmptyStorage);

        if (product.StorageQuantity < quantity)
            return Result.Failure(ProductErrors.InvalidQuantity);

        product.StorageQuantity -= quantity;

        await _unitOfWork.CompleteAsync(cancellationToken);

        return Result.Success();
    }

    private static string GetName(IFormFile image)
        => $"{Path.GetFileNameWithoutExtension(image.FileName)}-{Guid.CreateVersion7()}{Path.GetExtension(image.FileName)}";
}
