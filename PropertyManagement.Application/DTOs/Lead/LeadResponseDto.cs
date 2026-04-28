using System;
using System.Collections.Generic;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.DTOs.Lead;

public class LeadResponseDto
{
    public int Id { get; set; }
    public int? PropertyId { get; set; }
    public string PropertyName { get; set; } = string.Empty;
    public string PropertyAddress { get; set; } = string.Empty;
    public string PropertyType { get; set; } = string.Empty;
    public string OwnerNationalId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public LeadIntent Intent { get; set; }
    public decimal ListedPrice { get; set; }
    public LeadStatus Status { get; set; }
    public DateTime? PreferredContactAt { get; set; }
    public DateTime? LastContactedAt { get; set; }
    public int? AssignedToUserId { get; set; }
    public string? AssignedToUsername { get; set; }
    public List<LeadImageResponseDto> Images { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
