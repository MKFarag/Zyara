namespace Application.Interfaces.Application;

public interface IUserService
{
    Task<Result<UserProfileResponse>> GetProfileAsync(string id, CancellationToken cancellationToken = default);
    Task<Result> UpdateProfileAsync(string id, UpdateProfileRequest request, CancellationToken cancellationToken = default);
    Task<Result> ChangePasswordAsync(string id, ChangePasswordRequest request, CancellationToken cancellationToken = default);
    Task<Result> ChangeEmailRequestAsync(string id, string newEmail, CancellationToken cancellationToken = default);
    Task<Result> ConfirmChangeEmailAsync(string id, ConfirmChangeEmailRequest request, CancellationToken cancellationToken = default);
}
