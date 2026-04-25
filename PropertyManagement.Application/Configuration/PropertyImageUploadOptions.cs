namespace PropertyManagement.Application.Configuration;

public class PropertyImageUploadOptions
{
    public string RootPath { get; set; } = "uploads/properties";
    public string PublicBasePath { get; set; } = "/uploads/properties";
    public long MaxSizeBytes { get; set; } = 10 * 1024 * 1024;
    public string[] AllowedExtensions { get; set; } = [".jpg", ".jpeg", ".png", ".webp"];
    public string[] AllowedMimeTypes { get; set; } = ["image/jpeg", "image/png", "image/webp"];
}
