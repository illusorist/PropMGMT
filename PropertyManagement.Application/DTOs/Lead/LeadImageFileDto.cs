using System.IO;

namespace PropertyManagement.Application.DTOs.Lead;

public class LeadImageFileDto
{
    public required Stream Stream { get; init; }
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
}
