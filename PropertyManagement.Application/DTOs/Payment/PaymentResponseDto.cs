using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.DTOs.Payment;

public class PaymentResponseDto
{
    public int Id { get; set; }
    public int ContractId { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
