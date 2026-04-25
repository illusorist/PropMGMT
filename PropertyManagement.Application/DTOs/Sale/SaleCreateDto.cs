using System;
using System.ComponentModel.DataAnnotations;

namespace PropertyManagement.Application.DTOs.Sale;

public class SaleCreateDto
{
    [Required] public int PropertyId { get; set; }
    [Required] public int BuyerClientId { get; set; }
    [Required] public decimal SalePrice { get; set; }
    [Required] public string DeedNumber { get; set; } = string.Empty;
    [Required] public DateTime SoldAt { get; set; }
}
