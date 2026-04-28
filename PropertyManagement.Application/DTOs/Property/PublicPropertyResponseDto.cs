using System;
using System.Collections.Generic;
using PropertyManagement.Application.DTOs.Amenity;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.DTOs.Property;

public class PublicPropertyResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal? SalePrice { get; set; }
    public decimal? RentPrice { get; set; }
    public string? PrimaryImageUrl { get; set; }
    public List<PublicPropertyImageDto> Images { get; set; } = new();
    public PropertyStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<AmenityResponseDto> Amenities { get; set; } = new();
}

public class PublicPropertyImageDto
{
    public int Id { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
    public int SortOrder { get; set; }
}