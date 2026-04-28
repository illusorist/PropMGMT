using System;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.DTOs.Lead;

public class LeadUpdateDto
{
    public LeadStatus Status { get; set; }
    public int? AssignedToUserId { get; set; }
    public string? Notes { get; set; }
    public decimal ListedPrice { get; set; }
    public DateTime? LastContactedAt { get; set; }
    public DateTime? PreferredContactAt { get; set; }
}
