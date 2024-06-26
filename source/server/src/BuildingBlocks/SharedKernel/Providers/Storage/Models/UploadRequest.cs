namespace SharedKernel.Providers.Storage.Models;

public class UploadRequest
{
    public string FileName { get; set; }

    public string FileExtension { get; set; }

    public string Prefix { get; set; }

    public Stream Stream { get; set; }

    public long Size { get; set; }
}