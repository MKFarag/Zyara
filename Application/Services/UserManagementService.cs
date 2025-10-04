namespace Application.Services;

public class UserManagementService(IUnitOfWork unitOfWork) : IUserManagementService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _unitOfWork.Users.GetAllProjectionWithRolesAsync<UserResponse>(false, cancellationToken);

    public async Task<Result<UserResponse>> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Users.FindByIdAsync(id, cancellationToken) is not { } user)
            return Result.Failure<UserResponse>(UserErrors.NotFound);

        var roles = await _unitOfWork.Users.GetRolesAsync(user);

        var response = (user, roles).Adapt<UserResponse>();

        return Result.Success(response);
    }

    public async Task<Result<UserResponse>> AddAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Users.EmailExistsAsync(request.Email, cancellationToken))
            return Result.Failure<UserResponse>(UserErrors.DuplicatedEmail);

        if (await _unitOfWork.Users.UserNameExistsAsync(request.UserName, cancellationToken))
            return Result.Failure<UserResponse>(UserErrors.DuplicatedUserName);

        var allowedRoles = await _unitOfWork.Roles.GetAllNamesAsync(false, false, cancellationToken);

        if (request.Roles.Except(allowedRoles).Any())
            return Result.Failure<UserResponse>(UserErrors.InvalidRoles);

        var user = request.Adapt<User>();

        var createResult = await _unitOfWork.Users.CreateAsync(user, request.Password, true);

        if (createResult.IsFailure)
            return Result.Failure<UserResponse>(createResult.Error);

        var addToRolesResult = await _unitOfWork.Users.AddToRolesAsync(user, allowedRoles);

        if (addToRolesResult.IsFailure)
            return Result.Failure<UserResponse>(addToRolesResult.Error);

        var response = (user, request.Roles).Adapt<UserResponse>();

        return Result.Success(response);
    }

    public async Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Users.EmailExistsAsync(request.Email, id, cancellationToken))
            return Result.Failure(UserErrors.DuplicatedEmail);

        if (await _unitOfWork.Users.UserNameExistsAsync(request.UserName, id, cancellationToken))
            return Result.Failure(UserErrors.DuplicatedUserName);

        var allowedRoles = await _unitOfWork.Roles.GetAllNamesAsync(false, false, cancellationToken);

        if (request.Roles.Except(allowedRoles).Any())
            return Result.Failure(UserErrors.InvalidRoles);

        if (await _unitOfWork.Users.FindByIdAsync(id, cancellationToken) is not { } user)
            return Result.Failure(UserErrors.NotFound);

        user = request.Adapt(user);

        var updateResult = await _unitOfWork.Users.UpdateAsync(user);

        if (updateResult.IsFailure)
            return updateResult;

        await _unitOfWork.Users.DeleteAllRolesAsync(user);

        var addToRolesResult = await _unitOfWork.Users.AddToRolesAsync(user, request.Roles);

        return addToRolesResult;
    }

    public async Task<Result> ToggleStatusAsync(string id, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Users.FindByIdAsync(id, cancellationToken) is not { } user)
            return Result.Failure(UserErrors.NotFound);

        await _unitOfWork.Users.ToggleStatusAsync(user, cancellationToken);

        return Result.Success();
    }

    public async Task<Result> UnlockAsync(string id, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Users.FindByIdAsync(id, cancellationToken) is not { } user)
            return Result.Failure(UserErrors.NotFound);

        if (!await _unitOfWork.Users.IsLockedOutAsync(user))
            return Result.Failure(UserErrors.NotLocked);

        var unlockResult = await _unitOfWork.Users.SetLockoutEndDateAsync(user, null);

        return unlockResult;
    }
}
