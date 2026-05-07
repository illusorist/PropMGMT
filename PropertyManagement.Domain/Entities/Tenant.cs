using System.Collections.Generic;

namespace PropertyManagement.Domain.Entities;

public class Tenant : BaseEntity
{
    public int? PropertyId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;

    public Property? Property { get; set; }
    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}
