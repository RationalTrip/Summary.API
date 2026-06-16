namespace Summary.Infrastructure.Configurations;

public class AzureLanguageServiceConfigurations
{
    public static string SectionName => "AzureLanguageService";

    public required int DocumentSizeLimit { get; set; }

    public required int DocumentPerBatchLimit { get; set; }
}
