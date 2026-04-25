using System.Collections.Generic;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Domain.Entities;

public class Property : BaseEntity
{
    public int OwnerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public PropertyStatus Status { get; set; } = PropertyStatus.Pending;

    public Owner Owner { get; set; } = null!;
    public ICollection<PropertyAmenity> PropertyAmenities { get; set; } = new List<PropertyAmenity>();
}
