namespace Application.Contracts.User;

public record UserResponse(
    string Id,
    string FullName,
    string Email,
    string UserName,
    bool IsDisabled,
    IEnumerable<string> Roles
);
