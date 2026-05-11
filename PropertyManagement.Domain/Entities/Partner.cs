using System;
using System.Collections.Generic;

namespace PropertyManagement.Domain.Entities;

public class Partner
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? NationalId { get; set; }
    public string? Notes { get; set; }
    public int? UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public ICollection<Lead> Leads { get; set; } = new List<Lead>();
}
