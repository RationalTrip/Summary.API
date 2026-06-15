namespace Summary.Infrastructure.Configurations;

public class AzureLanguageServiceConfigurations
{
    public static string SectionName => "AzureLanguageService";

    public required string Endpoint { get; set; }

    public required string ApiKey { get; set; }
}
