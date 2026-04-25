using System.Collections.Generic;

namespace PropertyManagement.Domain.Entities;

public class BuyerClient : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;

    public ICollection<PropertySale> Sales { get; set; } = new List<PropertySale>();
}
