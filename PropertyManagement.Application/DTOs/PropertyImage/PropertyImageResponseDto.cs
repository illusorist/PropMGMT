namespace PropertyManagement.Application.DTOs.PropertyImage;

public class PropertyImageResponseDto
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
    public string Url { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
