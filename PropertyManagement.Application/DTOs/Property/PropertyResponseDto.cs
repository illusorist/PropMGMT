using System.Collections.Generic;
using PropertyManagement.Application.DTOs.Amenity;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.DTOs.Property;

public class PropertyResponseDto
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public PropertyStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<AmenityResponseDto> Amenities { get; set; } = new();
}
