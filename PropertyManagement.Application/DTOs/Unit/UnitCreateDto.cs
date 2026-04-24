using System.ComponentModel.DataAnnotations;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.DTOs.Unit;

public class UnitCreateDto
{
    [Required] public int PropertyId { get; set; }
    [Required] public string UnitNumber { get; set; } = string.Empty;
    [Required] public string Floor { get; set; } = string.Empty;
    [Required] public decimal Area { get; set; }
    [Required] public decimal BaseRent { get; set; }
    [Required] public UnitStatus Status { get; set; } = UnitStatus.Available;
}
