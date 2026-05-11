using System.ComponentModel.DataAnnotations;

namespace PropertyManagement.Application.DTOs.Partner;

public class CreatePartnerAccountDto
{
    [Required] public string Username { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}
