namespace PropertyManagement.Application.DTOs.Tenant;

public class TenantResponseDto
{
    public int Id { get; set; }
    public int? PropertyId { get; set; }
    public string? PropertyName { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
