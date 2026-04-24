using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.DTOs.Unit;

public class UnitResponseDto
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public string UnitNumber { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public decimal Area { get; set; }
    public decimal BaseRent { get; set; }
    public UnitStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
