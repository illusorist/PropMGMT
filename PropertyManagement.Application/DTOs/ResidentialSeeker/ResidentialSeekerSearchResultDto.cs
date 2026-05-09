using System.Collections.Generic;

namespace PropertyManagement.Application.DTOs.ResidentialSeeker;

public class ResidentialSeekerSearchResultDto
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<ResidentialSeekerDto> Items { get; set; } = [];
}
