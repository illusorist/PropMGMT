using System.ComponentModel.DataAnnotations;
using PropertyManagement.Domain.Enums;

namespace PropertyManagement.Application.DTOs.Property;

public class PropertyStatusUpdateDto
{
    [Required] public PropertyStatus Status { get; set; }
}
