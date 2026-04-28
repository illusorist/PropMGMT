using System;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Domain.Entities;

public class Lead : BaseEntity
{
    public int? PropertyId { get; set; }
    public string PropertyName { get; set; } = string.Empty;
    public string PropertyAddress { get; set; } = string.Empty;
    public string PropertyType { get; set; } = string.Empty;
    public string OwnerNationalId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public LeadIntent Intent { get; set; } = LeadIntent.Buy;
    public decimal ListedPrice { get; set; }
    public LeadStatus Status { get; set; } = LeadStatus.New;
    public DateTime? PreferredContactAt { get; set; }
    public DateTime? LastContactedAt { get; set; }
    public int? AssignedToUserId { get; set; }

    public Property? Property { get; set; }
    public User? AssignedToUser { get; set; }
    public ICollection<LeadImage> Images { get; set; } = new List<LeadImage>();
}