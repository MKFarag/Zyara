using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly string _imagesFolder = "/images";
    private readonly string _imagesPath;
    private readonly string _rootPath;

    public FileStorageService(IWebHostEnvironment webHostEnvironment)
    {
        _rootPath = webHostEnvironment.WebRootPath;
        _imagesPath = _rootPath + _imagesFolder;
    }

    public async Task SaveAsync(IFormFile file, FileTypes type, string fileName, CancellationToken cancellationToken = default)
    {
        string path = type switch
        {
            FileTypes.Image => Path.Combine(_imagesPath, fileName),
            _ => throw new InvalidOperationException("Invalid file type."),
        };

        using var stream = File.Create(path);
        await file.CopyToAsync(stream, cancellationToken);
    }

    public async Task RemoveAsync(string relativePath, CancellationToken cancellationToken = default)
    {
        var path = GetPath(relativePath);

        if (File.Exists(path))
            await Task.Run(() => File.Delete(path), cancellationToken);
    }

    public string GetRelativePath(string fileName, FileTypes type)
        => type switch
        {
            FileTypes.Image => Path.Combine(_imagesFolder, fileName).Replace("\\", "/"),
            _ => throw new InvalidOperationException("Invalid file type."),
        };

    private string GetPath(string relativePath)
    {
        if (!relativePath.StartsWith(_imagesFolder))
            throw new InvalidOperationException("Invalid path.");

        return Path.Combine(_rootPath, relativePath.TrimStart('/'));
    }
}
