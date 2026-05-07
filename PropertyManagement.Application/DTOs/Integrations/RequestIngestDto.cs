using System;
using System.ComponentModel.DataAnnotations;

namespace PropertyManagement.Application.DTOs.Integrations;

public class RequestIngestDto
{
    public DateTime? RequestDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Employee { get; set; } = string.Empty;
    public string Via { get; set; } = string.Empty;

    [Required]
    public string FullName { get; set; } = string.Empty;

    public string Nationality { get; set; } = string.Empty;
    public string Profession { get; set; } = string.Empty;
    public int? BedroomCount { get; set; }

    [Required]
    public string MobileNumber { get; set; } = string.Empty;

    [Required]
    public string RequestType { get; set; } = string.Empty;

    public decimal? MaxBudget { get; set; }
    public string PaymentType { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
