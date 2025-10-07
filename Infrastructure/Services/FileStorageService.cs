using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Services;

public class FileStorageService(IWebHostEnvironment webHostEnvironment) : IFileStorageService
{
    private readonly string _imagesPath = $"{webHostEnvironment.WebRootPath}/images";

    public async Task SaveFileAsync(IFormFile file, string path, CancellationToken cancellationToken = default)
    {
        using var stream = File.Create(path);
        await file.CopyToAsync(stream, cancellationToken);
    }

    public string ImagesPathCombiner(string imageName)
        => Path.Combine(_imagesPath, imageName);
}

