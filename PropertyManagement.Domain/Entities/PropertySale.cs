using System;

namespace PropertyManagement.Domain.Entities;

public class PropertySale : BaseEntity
{
    public int PropertyId { get; set; }
    public int BuyerClientId { get; set; }
    public decimal SalePrice { get; set; }
    public string DeedNumber { get; set; } = string.Empty;
    public DateTime SoldAt { get; set; }

    public Property Property { get; set; } = null!;
    public BuyerClient BuyerClient { get; set; } = null!;
}
