using System.Security.Claims;

namespace Presentation.Extensions;

public static class UserExtensions
{
    public static string? GetId(this ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.NameIdentifier);
}
