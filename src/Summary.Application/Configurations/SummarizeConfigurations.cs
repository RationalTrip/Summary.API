namespace Summary.Application.Configurations;

public class SummarizeConfigurations
{
    public static string SectionName => "Summarize";

    public required int MaxInputLength { get; set; }
}
