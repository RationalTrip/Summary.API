using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Summary.Core.Exceptions;
using Summary.Core.Interfaces;
using Summary.Infrastructure.Configurations;
using Summary.Infrastructure.Helpers;

namespace Summary.Infrastructure.Callers;

public class AzureLanguageServiceCaller : ILanguageServiceCaller
{
    private readonly TextAnalyticsClient _textAnalyticsClient;
    private readonly ILogger<AzureLanguageServiceCaller> _logger;
    private readonly AzureLanguageServiceConfigurations _languageServiceConfiguration;

    public AzureLanguageServiceCaller(
        TextAnalyticsClient textAnalyticsClient,
        ILogger<AzureLanguageServiceCaller> logger,
        IOptions<AzureLanguageServiceConfigurations> languageServiceConfigurationOptions)
    {
        _textAnalyticsClient = textAnalyticsClient;
        _logger = logger;
        _languageServiceConfiguration = languageServiceConfigurationOptions.Value;
    }

    public async Task<string> AbstractiveSummarizeAsync(string text, CancellationToken cancellationToken)
    {
        var result = new List<string>();

        var chunks = PreferParagraphTextChunker.Split(text, _languageServiceConfiguration.DocumentSizeLimit);

        foreach (var chunk in chunks)
        {
            var operation = await _textAnalyticsClient.AbstractiveSummarizeAsync(
            WaitUntil.Completed,
            [chunk],
            options: new AbstractiveSummarizeOptions { IncludeStatistics = true, },
            cancellationToken: cancellationToken);

            await foreach (var page in operation.Value)
            {
                foreach (var documentResult in page)
                {
                    if (documentResult is null)
                        continue;

                    if (documentResult.HasError)
                    {
                        _logger.LogError(
                            "Azure Language Service returned an error for a document. ErrorCode: {ErrorCode}, Message: {Message}",
                            documentResult.Error.ErrorCode,
                            documentResult.Error.Message);

                        throw new SummaryGeneralException("Azure Language Service returned an error.", debugDetails: new { documentResult.Error.ErrorCode, documentResult.Error.Message });
                    }

                    _logger.LogInformation(
                        "Azure Language Service usage. Characters: {CharacterCount}, Transactions: {TransactionCount}",
                        documentResult.Statistics.CharacterCount,
                        documentResult.Statistics.TransactionCount);

                    var summaries = documentResult.Summaries.Where(s => !string.IsNullOrWhiteSpace(s.Text)).Select(s => s.Text);

                    if (summaries.Any())
                        result.AddRange(summaries);
                }
            }
        }

        return string.Join(" ", result);
    }
}
