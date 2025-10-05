namespace Application.Contracts.Role;

public record RoleResponse(
    string Id,
    string Name,
    bool IsDisabled
);
