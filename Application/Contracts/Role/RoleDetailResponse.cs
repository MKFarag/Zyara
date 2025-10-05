namespace Application.Contracts.Role;

public record RoleDetailResponse(
    string Id,
    string Name,
    bool IsDisabled,
    IEnumerable<string> Permissions
);
