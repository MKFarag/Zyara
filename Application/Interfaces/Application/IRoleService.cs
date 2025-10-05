namespace Application.Interfaces.Application;

public interface IRoleService
{
    Task<IEnumerable<RoleResponse>> GetAllAsync(bool includeDisabled, CancellationToken cancellationToken = default);
    Task<Result<RoleDetailResponse>> GetAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<RoleDetailResponse>> AddAsync(RoleRequest request);
    Task<Result> UpdateAsync(string id, RoleRequest request);
    Task<Result> ToggleStatusAsync(string id, CancellationToken cancellationToken = default);
}
