using System;

namespace PropertyManagement.Application.DTOs.Integrations;

public class RequestSearchQueryDto
{
    public string? Q { get; set; }
    public string? Status { get; set; }
    public string? Employee { get; set; }
    public string? RequestType { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; }
    public string? SortDir { get; set; }
}
