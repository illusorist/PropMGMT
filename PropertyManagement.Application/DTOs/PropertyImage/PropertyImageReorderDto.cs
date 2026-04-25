using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PropertyManagement.Application.DTOs.PropertyImage;

public class PropertyImageReorderDto
{
    [Required] public List<int> ImageIdsInOrder { get; set; } = new();
}
