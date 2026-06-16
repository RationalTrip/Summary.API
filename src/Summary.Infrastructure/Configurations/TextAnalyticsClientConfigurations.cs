namespace Summary.Infrastructure.Configurations;

public class TextAnalyticsClientConfigurations
{
    public static string SectionName => "TextAnalyticsClient";

    public required string Endpoint { get; set; }

    public required string ApiKey { get; set; }
}
