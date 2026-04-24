using System.Collections.Generic;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Domain.Entities;

public class Unit : BaseEntity
{
    public int PropertyId { get; set; }
    public string UnitNumber { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public decimal Area { get; set; }
    public decimal BaseRent { get; set; }
    public UnitStatus Status { get; set; } = UnitStatus.Available;

    public Property Property { get; set; } = null!;
    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}
