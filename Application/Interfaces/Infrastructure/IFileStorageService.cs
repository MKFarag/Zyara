namespace Application.Interfaces.Infrastructure;

public interface IFileStorageService
{
    Task SaveAsync(IFormFile file, FileTypes type, string fileName, CancellationToken cancellationToken = default);
    Task RemoveAsync(string relativePath, CancellationToken cancellationToken = default);
    string GetRelativePath(string fileName, FileTypes type);
}
