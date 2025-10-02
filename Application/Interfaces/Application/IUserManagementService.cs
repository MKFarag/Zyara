namespace Application.Interfaces.Application;

public interface IUserManagementService
{
    Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<UserResponse>> GetAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<UserResponse>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
}
