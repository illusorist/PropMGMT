namespace PropertyManagement.Application.DTOs.Integrations;

public class RequestIngestResponseDto
{
    public int Id { get; set; }
    public string Message { get; set; } = "Request ingested successfully.";
}
