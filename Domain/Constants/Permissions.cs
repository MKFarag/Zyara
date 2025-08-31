namespace Domain.Constants;

public static class Permissions
{
    public static string Type { get; } = nameof(Permissions).ToLower();

	#region Roles

	//public const string GetRoles = "role:read";

	#endregion

	public static IList<string?> GetAll()
		=> [.. typeof(Permissions).GetFields().Select(x => x.GetValue(x) as string)];
}
