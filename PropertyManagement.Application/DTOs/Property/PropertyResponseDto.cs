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
    public string? Region { get; set; }
    public string? City { get; set; }
    public string? District { get; set; }
    public string ListingType { get; set; } = "Rental";
    public decimal? SalePrice { get; set; }
    public decimal? RentPrice { get; set; }
    public string? DeedNumber { get; set; }
    public string? PrimaryImageUrl { get; set; }
    public PropertyStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<AmenityResponseDto> Amenities { get; set; } = new();
}
