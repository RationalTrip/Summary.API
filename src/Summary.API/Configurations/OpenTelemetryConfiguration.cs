namespace Summary.API.Configurations;

public class OpenTelemetryConfiguration
{
    public const string SectionName = "ApplicationInsights";

    public required string ServiceName { get; set; }

    public double SamplingRatio { get; set; }

    public required string ConnectionString { get; set; }
}
