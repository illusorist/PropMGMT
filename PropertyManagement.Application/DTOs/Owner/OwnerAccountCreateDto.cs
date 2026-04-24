using System.ComponentModel.DataAnnotations;

namespace PropertyManagement.Application.DTOs.Owner;

public class OwnerAccountCreateDto
{
    [Required] public string Username { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}
