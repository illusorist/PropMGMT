using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.Application.Interfaces;

public interface IPropertyImageStorage
{
    Task<PropertyImageStoredFile> SaveAsync(int propertyId, Stream stream, string originalFileName, string contentType, long sizeBytes, CancellationToken cancellationToken = default);
    Task DeleteAsync(string relativePath, CancellationToken cancellationToken = default);
    string BuildPublicUrl(string relativePath);
}

public class PropertyImageStoredFile
{
    public string StoredFileName { get; set; } = string.Empty;
    public string RelativePath { get; set; } = string.Empty;
}
