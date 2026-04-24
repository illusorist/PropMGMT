using System.ComponentModel.DataAnnotations;

namespace PropertyManagement.Application.DTOs.Owner;

public class OwnerCreateDto
{
    [Required] public string FullName { get; set; } = string.Empty;
    [Required] public string Phone { get; set; } = string.Empty;
    [Required] public string Email { get; set; } = string.Empty;
    [Required] public string NationalId { get; set; } = string.Empty;
}
