using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Application.Services;

public class UserService(IUnitOfWork unitOfWork, IEmailTemplateService emailTemplateService) : IUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IEmailTemplateService _emailTemplateService = emailTemplateService;
    private readonly int _minDaysBetweenChanges = 30;

    public async Task<Result<UserProfileResponse>> GetProfileAsync(string id, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Users.FindByIdAsync(id, cancellationToken) is not { } user)
            return Result.Failure<UserProfileResponse>(UserErrors.NotFound);

        return Result.Success(user.Adapt<UserProfileResponse>());
    }

    public async Task<Result> UpdateProfileAsync(string id, UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Users.FindByIdAsync(id, cancellationToken) is not { } user)
            return Result.Failure(UserErrors.NotFound);

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        var updateResult = await _unitOfWork.Users.UpdateAsync(user);

        return updateResult;
    }

    public async Task<Result> ChangePasswordAsync(string id, ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Users.FindByIdAsync(id, cancellationToken) is not { } user)
            return Result.Failure(UserErrors.NotFound);

        var changePasswordResult = await _unitOfWork.Users.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        return changePasswordResult;
    }

    public async Task<Result> ChangeEmailRequestAsync(string id, string newEmail, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Users.EmailExistsAsync(newEmail, id, cancellationToken))
            return Result.Failure(UserErrors.DuplicatedEmail);

        if (await _unitOfWork.Users.FindByIdAsync(id, cancellationToken) is not { } user)
            return Result.Failure(UserErrors.NotFound);

        if (string.Equals(user.Email, newEmail, StringComparison.OrdinalIgnoreCase))
            return Result.Failure(UserErrors.SameEmail);

        var token = await _unitOfWork.Users.GenerateChangeEmailTokenAsync(user, newEmail);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        _emailTemplateService.SendConfirmationLink(user, token);

        return Result.Success();
    }

    public async Task<Result> ConfirmChangeEmailAsync(string id, ConfirmChangeEmailRequest request, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Users.FindByIdAsync(id, cancellationToken) is not { } user)
            return Result.Failure(UserErrors.NotFound);

        var oldEmail = user.Email;
        Result result;

        try
        {
            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            result = await _unitOfWork.Users.ChangeEmailAsync(user, request.NewEmail, token);
        }
        catch (FormatException)
        {
            result = Result.Failure(UserErrors.InvalidToken);
        }

        if (result.IsFailure)
            return Result.Failure(result.Error);

        _emailTemplateService.SendChangeEmailNotification(user, oldEmail, DateTime.UtcNow);

        return Result.Success();
    }

    public async Task<Result> ChangeUserNameAsync(string id, string newUserName, CancellationToken cancellationToken = default)
    {
        if (await _unitOfWork.Users.UserNameExistsAsync(newUserName, id, cancellationToken))
            return Result.Failure(UserErrors.DuplicatedUserName);

        if (await _unitOfWork.Users.FindByIdAsync(id, cancellationToken) is not { } user)
            return Result.Failure(UserErrors.NotFound);

        if (string.Equals(user.UserName, newUserName, StringComparison.OrdinalIgnoreCase))
            return Result.Failure(UserErrors.SameUserName);

        if (await _unitOfWork.Users.IsChangeUserNameAvailable(user))
            return Result.Failure(UserErrors.UserNameChangeNotAllowed);

        var changeUserNameResult = await _unitOfWork.Users.ChangeUserNameAsync(user, newUserName, _minDaysBetweenChanges);

        return changeUserNameResult;
    }
}
