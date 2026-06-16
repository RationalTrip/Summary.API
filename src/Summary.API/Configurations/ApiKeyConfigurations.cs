namespace Summary.API.Configurations;

public class ApiKeyConfigurations
{
    public static string SectionName => "ApiKey";

    public required string Key { get; set; }
}
