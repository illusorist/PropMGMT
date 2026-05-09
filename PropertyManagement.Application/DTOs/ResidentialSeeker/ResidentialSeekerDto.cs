using System;

namespace PropertyManagement.Application.DTOs.ResidentialSeeker;

public class ResidentialSeekerDto
{
    public int Id { get; set; }
    public string? SerialNumber { get; set; }
    public string? RequestDate { get; set; }
    public string? Status { get; set; }
    public string? Employee { get; set; }
    public string? Receiver { get; set; }
    public string? SourceChannel { get; set; }
    public string? Mobile { get; set; }
    public string? FullName { get; set; }
    public string? Nationality { get; set; }
    public string? Profession { get; set; }
    public string? FamilyCount { get; set; }
    public string? RequestDescription { get; set; }
    public string? MaxBudget { get; set; }
    public string? PaymentType { get; set; }
    public string? PreferredLocation { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
