using System;
using System.Collections.Generic;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Domain.Entities;

public class Contract : BaseEntity
{
    public int UnitId { get; set; }
    public int TenantId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal MonthlyRent { get; set; }
    public ContractStatus Status { get; set; } = ContractStatus.Pending;

    public Unit Unit { get; set; } = null!;
    public Tenant Tenant { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
