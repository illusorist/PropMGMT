namespace PropertyManagement.Domain.Entities;

public class Property : BaseEntity
{
    public int OwnerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public Owner Owner { get; set; } = null!;
}
