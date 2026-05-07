namespace PropertyManagement.Application.Configuration;

public class RequestsIngestionOptions
{
    public bool Enabled { get; set; } = true;
    public string ApiKey { get; set; } = string.Empty;
    public string HeaderName { get; set; } = "X-API-Key";
}
