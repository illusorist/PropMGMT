using System;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.DTOs.Lead;

public class LeadResponseDto
{
    public int Id { get; set; }
    public int? PropertyId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public LeadIntent Intent { get; set; }
    public LeadStatus Status { get; set; }
    public DateTime? PreferredContactAt { get; set; }
    public DateTime? LastContactedAt { get; set; }
    public int? AssignedToUserId { get; set; }
    public string? AssignedToUsername { get; set; }
    public DateTime CreatedAt { get; set; }
}
