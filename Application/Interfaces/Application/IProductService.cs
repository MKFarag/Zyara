using Application.Contracts.Product;

namespace Application.Interfaces.Application;

public interface IProductService
{
    Task<IPaginatedList<ProductResponse>> GetAllAsync(RequestFilters filters, CancellationToken cancellationToken = default);
}
