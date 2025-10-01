namespace Application.Contracts.User;

public record UserProfileResponse(
    string FirstName,
    string LastName,
    string Email,
    string UserName
);
