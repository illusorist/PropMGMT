using System.ComponentModel.DataAnnotations;

namespace PropertyManagement.Application.DTOs.Partner;

public class UpdatePartnerDto
{
    [Required] public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? NationalId { get; set; }
    public string? Notes { get; set; }
}
