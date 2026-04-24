using System.Collections.Generic;

namespace PropertyManagement.Domain.Entities;

public class Owner : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;

    public ICollection<Property> Properties { get; set; } = new List<Property>();
}
