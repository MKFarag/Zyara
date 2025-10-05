namespace Application.Services;

public class RoleService(IUnitOfWork unitOfWork) : IRoleService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool includeDisabled, CancellationToken cancellationToken = default)
    {
        var roles = await _unitOfWork.Roles.GetAllAsync(false, includeDisabled, cancellationToken);

        return roles.Adapt<IEnumerable<RoleResponse>>();
    }

    public async Task<Result<RoleDetailResponse>> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Roles.GetAsync(id, cancellationToken) is not { } role)
            return Result.Failure<RoleDetailResponse>(RoleErrors.NotFound);

        var permissions = await _unitOfWork.Roles.GetClaimsAsync(id, Permissions.Type, cancellationToken);

        var response = (role, permissions).Adapt<RoleDetailResponse>();

        return Result.Success(response);
    }

    public async Task<Result<RoleDetailResponse>> AddAsync(RoleRequest request)
    {
        if (await _unitOfWork.Roles.NameExistsAsync(request.Name))
            return Result.Failure<RoleDetailResponse>(RoleErrors.DuplicatedName);

        var allowedPermissions = Permissions.GetAll();

        if (request.Permissions.Except(allowedPermissions).Any())
            return Result.Failure<RoleDetailResponse>(RoleErrors.InvalidPermissions);

        var role = request.Adapt<Role>();

        var createResult = await _unitOfWork.Roles.CreateAsync(role);

        if (createResult.IsFailure)
            return Result.Failure<RoleDetailResponse>(createResult.Error);

        await _unitOfWork.Roles.AddClaimsAsync(role.Id, Permissions.Type, request.Permissions);

        var response = (role, request.Permissions).Adapt<RoleDetailResponse>();

        return Result.Success(response);
    }

    public async Task<Result> UpdateAsync(string id, RoleRequest request)
    {
        if (await _unitOfWork.Roles.NameExistsAsync(request.Name, id))
            return Result.Failure(RoleErrors.DuplicatedName);

        if (await _unitOfWork.Roles.GetAsync(id) is not { } role)
            return Result.Failure(RoleErrors.NotFound);

        var allowedPermissions = Permissions.GetAll();

        if (request.Permissions.Except(allowedPermissions).Any())
            return Result.Failure(RoleErrors.InvalidPermissions);

        role = request.Adapt(role);

        var updateResult = await _unitOfWork.Roles.UpdateAsync(role);

        if (updateResult.IsFailure)
            return updateResult;

        var currentPermissions = await _unitOfWork.Roles.GetClaimsAsync(id, Permissions.Type);

        var newPermissions = request.Permissions.Except(currentPermissions);

        var removedPermissions = currentPermissions.Except(request.Permissions);

        if (removedPermissions.Any())
            await _unitOfWork.Roles.DeleteClaimsAsync(id, newPermissions);

        if (newPermissions.Any())
            await _unitOfWork.Roles.AddClaimsAsync(id, Permissions.Type, newPermissions);

        return Result.Success();
    }

    public async Task<Result> ToggleStatusAsync(string id, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Roles.GetAsync(id, cancellationToken) is not { } role)
            return Result.Failure(RoleErrors.NotFound);

        await _unitOfWork.Roles.ToggleStatusAsync(role, cancellationToken);

        return Result.Success();
    }
}
