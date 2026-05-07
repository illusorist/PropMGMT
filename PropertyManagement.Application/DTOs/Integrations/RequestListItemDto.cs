using System;

namespace PropertyManagement.Application.DTOs.Integrations;

public class RequestListItemDto
{
    public int Id { get; set; }
    public DateTime? RequestDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Employee { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
