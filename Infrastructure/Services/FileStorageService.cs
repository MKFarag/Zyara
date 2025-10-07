using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Services;

public class FileStorageService(IWebHostEnvironment webHostEnvironment) : IFileStorageService
{
    private readonly string _imagesPath = $"{webHostEnvironment.WebRootPath}/images";

    public async Task SaveAsync(IFormFile file, string path, CancellationToken cancellationToken = default)
    {
        using var stream = File.Create(path);
        await file.CopyToAsync(stream, cancellationToken);
    }

    public async Task RemoveAsync(string path, CancellationToken cancellationToken = default)
    {
        if (!path.StartsWith(_imagesPath))
            throw new InvalidOperationException("Invalid image path.");

        if (File.Exists(path))
            await Task.Run(() => File.Delete(path), cancellationToken);
    }

    public string ImagesPathCombiner(string imageName)
        => Path.Combine(_imagesPath, imageName);
}

