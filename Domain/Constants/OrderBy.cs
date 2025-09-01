namespace Domain.Constants;

public static class OrderBy
{
    public const string Ascending = "ASC";
    public const string Descending = "DESC";

    public static bool IsValid(string orderByType)
        => orderByType == Ascending || orderByType == Descending;
}