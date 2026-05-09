namespace PropertyManagement.Application.DTOs.CommercialListing;

public class CommercialListingSearchQueryDto
{
    public string? Q { get; set; }
    public string? Status { get; set; }
    public string? Employee { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; }
    public string? SortDir { get; set; }
}
