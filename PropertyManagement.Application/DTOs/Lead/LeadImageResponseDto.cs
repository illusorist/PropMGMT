namespace PropertyManagement.Application.DTOs.Lead;

public class LeadImageResponseDto
{
    public int Id { get; set; }
    public int LeadId { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}