namespace Application.Interfaces.Infrastructure;

public interface IFileStorageService
{
    Task<string> SaveAsync(IFormFile file, FileTypes fileType, string fileName, CancellationToken cancellationToken = default);
    Task RemoveAsync(string relativePath, CancellationToken cancellationToken = default);
}
