using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PropertyManagement.Application.Configuration;
using PropertyManagement.Application.Interfaces;

namespace PropertyManagement.Infrastructure.Storage;

public class DiskLeadImageStorage : ILeadImageStorage
{
    private readonly LeadImageUploadOptions _options;

    public DiskLeadImageStorage(IOptions<LeadImageUploadOptions> options)
    {
        _options = options.Value;
    }

    public async Task<PropertyImageStoredFile> SaveAsync(int leadId, Stream stream, string originalFileName, string contentType, long sizeBytes, CancellationToken cancellationToken = default)
    {
        if (sizeBytes <= 0)
            throw new InvalidOperationException("Image file is empty");
        if (sizeBytes > _options.MaxSizeBytes)
            throw new InvalidOperationException($"Image exceeds maximum size of {_options.MaxSizeBytes} bytes");

        var ext = Path.GetExtension(originalFileName).ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(ext) || !_options.AllowedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
            throw new InvalidOperationException("Unsupported image extension");
        if (!_options.AllowedMimeTypes.Contains(contentType, StringComparer.OrdinalIgnoreCase))
            throw new InvalidOperationException("Unsupported image MIME type");

        var root = GetRootPath();
        var leadFolder = Path.Combine(root, leadId.ToString());
        Directory.CreateDirectory(leadFolder);

        var storedFileName = $"{Guid.NewGuid():N}{ext}";
        var fullPath = Path.Combine(leadFolder, storedFileName);
        await using var fileStream = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        await stream.CopyToAsync(fileStream, cancellationToken);

        var relativePath = Path.Combine(leadId.ToString(), storedFileName).Replace("\\", "/");
        return new PropertyImageStoredFile
        {
            StoredFileName = storedFileName,
            RelativePath = relativePath
        };
    }

    public Task DeleteAsync(string relativePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return Task.CompletedTask;

        var safePath = relativePath.Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.Combine(GetRootPath(), safePath);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
        return Task.CompletedTask;
    }

    public string GetPhysicalPath(string relativePath)
    {
        var safePath = relativePath.Replace('/', Path.DirectorySeparatorChar);
        return Path.Combine(GetRootPath(), safePath);
    }

    private string GetRootPath()
    {
        if (Path.IsPathRooted(_options.RootPath))
            return _options.RootPath;

        return Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), _options.RootPath));
    }
}
