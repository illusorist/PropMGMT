namespace PropertyManagement.Domain.Entities;

public class LeadImage : BaseEntity
{
    public int LeadId { get; set; }
    public string StoredFileName { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public string RelativePath { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }

    public Lead Lead { get; set; } = null!;
}