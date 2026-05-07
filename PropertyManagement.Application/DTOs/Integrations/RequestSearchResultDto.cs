using System.Collections.Generic;

namespace PropertyManagement.Application.DTOs.Integrations;

public class RequestSearchResultDto
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<RequestListItemDto> Items { get; set; } = [];
}
