namespace Domain.Entities;

public sealed class UploadedFile
{
    public string Id { get; set; } = Guid.CreateVersion7().ToString();
    public string FileName { get; set; } = string.Empty;
    public string StoredFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
}
