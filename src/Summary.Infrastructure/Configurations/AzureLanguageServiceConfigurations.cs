namespace Summary.Infrastructure.Configurations;

public class AzureLanguageServiceConfigurations
{
    public static string SectionName => "AzureLanguageService";

    public required string Endpoint { get; set; }

    public required string ApiKey { get; set; }

    /// <summary>
    /// Maximum number of characters per document sent to Azure Language Service.
    /// Azure's hard limit is 5 120 characters per document.
    /// </summary>
    public int MaxDocumentCharacterLength { get; set; } = 5000;

    /// <summary>
    /// Maximum number of concurrent requests sent to Azure Language Service.
    /// </summary>
    public int MaxDegreeOfParallelism { get; set; } = 4;
}
