using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.DTOs.Owner;

public class OwnerStatsDto
{
    public int OwnerId { get; set; }
    public int TotalProperties { get; set; }
    public int PendingProperties { get; set; }
    public int ApprovedProperties { get; set; }
    public int RejectedProperties { get; set; }
    public int SoldProperties { get; set; }
    public int TotalContracts { get; set; }
    public int ActiveContracts { get; set; }
    public int PendingContracts { get; set; }
    public int ExpiredContracts { get; set; }
    public int TerminatedContracts { get; set; }
}
