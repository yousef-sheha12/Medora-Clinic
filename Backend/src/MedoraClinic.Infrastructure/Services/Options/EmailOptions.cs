namespace MedoraClinic.Infrastructure.Services.Options;

public sealed class EmailOptions
{
    public const string SectionName = "Email";

    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string Username { get; set; } = string.Empty;
    public string AppPassword { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
    public string FrontendBaseUrl { get; set; } = string.Empty;
}
