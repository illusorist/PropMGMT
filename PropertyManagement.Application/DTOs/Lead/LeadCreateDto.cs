using System;
using System.ComponentModel.DataAnnotations;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.DTOs.Lead;

public class LeadCreateDto
{
    public int? PropertyId { get; set; }
    [Required] public string FullName { get; set; } = string.Empty;
    [Required] public string Phone { get; set; } = string.Empty;
    [Required] public string Email { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    [Required] public LeadIntent Intent { get; set; }
    public DateTime? PreferredContactAt { get; set; }
}
