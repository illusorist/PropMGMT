using System;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Domain.Entities;

public class Payment : BaseEntity
{
    public int ContractId { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public Contract Contract { get; set; } = null!;
}
