using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PropertyManagement.Application.DTOs.Property;

public class PropertyCreateDto
{
    [Required] public int OwnerId { get; set; }
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Address { get; set; } = string.Empty;
    [Required] public string Type { get; set; } = string.Empty;
    public decimal? SalePrice { get; set; }
    public decimal? RentPrice { get; set; }
    public List<int> AmenityIds { get; set; } = new();
}
