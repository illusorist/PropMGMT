using System.Collections.Generic;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Domain.Entities;

public class Property : BaseEntity
{
    public int OwnerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal? SalePrice { get; set; }
    public decimal? RentPrice { get; set; }
    public PropertyStatus Status { get; set; } = PropertyStatus.Pending;

    public Owner Owner { get; set; } = null!;
    public ICollection<PropertyAmenity> PropertyAmenities { get; set; } = new List<PropertyAmenity>();
    public ICollection<PropertySale> Sales { get; set; } = new List<PropertySale>();
    public ICollection<Lead> Leads { get; set; } = new List<Lead>();
    public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
}
