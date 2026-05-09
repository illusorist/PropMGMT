using System.Collections.Generic;

namespace PropertyManagement.Application.DTOs.CommercialListing;

public class CommercialListingSearchResultDto
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<CommercialListingDto> Items { get; set; } = [];
}
