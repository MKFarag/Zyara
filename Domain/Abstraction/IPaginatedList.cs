namespace Domain.Abstraction;

/// <summary>Represents a paginated list of items of type <typeparamref name="T"/></summary>
/// <typeparam name="T">The type of elements in the list</typeparam>
public interface IPaginatedList<T> where T : class
{
    public List<T> Items { get; }
    int PageNumber { get; }
    int TotalPages { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
}
