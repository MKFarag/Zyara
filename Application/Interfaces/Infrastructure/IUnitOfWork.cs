namespace Application.Interfaces.Infrastructure;

public interface IUnitOfWork : IDisposable
{


    /// <summary>Save changes to the database</summary>
    /// <returns>The number of state entries written to the database</returns>
    int Complete();

    /// <summary>Save changes to the database</summary>
    /// <returns>The number of state entries written to the database</returns>
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}
