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

    public async Task<string> SaveAsync(IFormFile file, FileTypes fileType, string fileName, CancellationToken cancellationToken = default)
    {
        string path = fileType switch
        {
            FileTypes.Image => Path.Combine(_imagesPath, fileName),
            _ => throw new InvalidOperationException("Invalid file type."),
        };

        using var stream = File.Create(path);
        await file.CopyToAsync(stream, cancellationToken);

        return GetRelativePath(path);
    }

    public async Task RemoveAsync(string relativePath, CancellationToken cancellationToken = default)
    {
        if (!relativePath.StartsWith(_imagesFolder))
            throw new InvalidOperationException("Invalid path.");

        var path = _rootPath + relativePath;

        if (File.Exists(path))
            await Task.Run(() => File.Delete(path), cancellationToken);
    }

    private string GetRelativePath(string fullPath)
        => fullPath.Replace(_rootPath, "").Replace("\\", "/");
}

