namespace Application.Interfaces.Infrastructure;

public interface IFileStorageService
{
    Task SaveFileAsync(IFormFile file, string path, CancellationToken cancellationToken = default);
    string ImagesPathCombiner(string imageName);
}
