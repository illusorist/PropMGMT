using System.ComponentModel.DataAnnotations;

namespace PropertyManagement.Application.DTOs.Amenity;

public class AmenityCreateDto
{
    [Required] public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
