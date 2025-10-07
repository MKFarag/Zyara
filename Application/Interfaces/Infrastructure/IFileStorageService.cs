namespace Application.Interfaces.Infrastructure;

public interface IFileStorageService
{
    Task SaveAsync(IFormFile file, string path, CancellationToken cancellationToken = default);
    Task RemoveAsync(string path, CancellationToken cancellationToken = default);
    string ImagesPathCombiner(string imageName);
}
