using System.ComponentModel.DataAnnotations;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.DTOs.Payment;

public class PaymentCreateDto
{
    [Required] public int ContractId { get; set; }
    [Required] public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    [Required] public decimal Amount { get; set; }
    [Required] public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
}
