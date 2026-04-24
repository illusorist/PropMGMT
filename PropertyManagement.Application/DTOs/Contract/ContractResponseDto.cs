using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.DTOs.Contract;

public class ContractResponseDto
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public int TenantId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal MonthlyRent { get; set; }
    public ContractStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
