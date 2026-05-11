using System.ComponentModel.DataAnnotations;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.DTOs.Partner;

public class SubmitPartnerLeadDto
{
    [Required] public string PropertyName { get; set; } = string.Empty;
    [Required] public string Address { get; set; } = string.Empty;
    [Required] public string Type { get; set; } = string.Empty;
    [Required] public LeadIntent Intent { get; set; }
    [Required] public decimal ListedPrice { get; set; }
    public string? Notes { get; set; }
}
