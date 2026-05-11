using System;

namespace PropertyManagement.Application.DTOs.Partner;

public class PartnerResponseDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? NationalId { get; set; }
    public string? Notes { get; set; }
    public int? UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
