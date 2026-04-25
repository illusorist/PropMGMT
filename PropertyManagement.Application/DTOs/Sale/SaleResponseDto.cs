using System;

namespace PropertyManagement.Application.DTOs.Sale;

public class SaleResponseDto
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public int BuyerClientId { get; set; }
    public decimal SalePrice { get; set; }
    public string DeedNumber { get; set; } = string.Empty;
    public DateTime SoldAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
