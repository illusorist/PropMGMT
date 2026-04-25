using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyManagement.Application.Interfaces;

public interface ILeadImageStorage
{
    Task<PropertyImageStoredFile> SaveAsync(int leadId, Stream stream, string originalFileName, string contentType, long sizeBytes, CancellationToken cancellationToken = default);
    Task DeleteAsync(string relativePath, CancellationToken cancellationToken = default);
    string GetPhysicalPath(string relativePath);
}
