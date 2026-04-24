using System.ComponentModel.DataAnnotations;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.DTOs.Contract;

public class ContractCreateDto
{
    [Required] public int UnitId { get; set; }
    [Required] public int TenantId { get; set; }
    [Required] public DateTime StartDate { get; set; }
    [Required] public DateTime EndDate { get; set; }
    [Required] public decimal MonthlyRent { get; set; }
    [Required] public ContractStatus Status { get; set; } = ContractStatus.Pending;
}
