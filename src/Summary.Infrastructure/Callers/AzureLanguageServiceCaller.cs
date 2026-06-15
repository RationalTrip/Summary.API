using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.Options;
using Summary.Core.Exceptions;
using Summary.Core.Interfaces;
using Summary.Infrastructure.Configurations;

namespace Summary.Infrastructure.Callers;

public class AzureLanguageServiceCaller : ILanguageServiceCaller
{
    private readonly TextAnalyticsClient _textAnalyticsClient;

    public AzureLanguageServiceCaller(IOptions<AzureLanguageServiceConfigurations> options)
    {
        var config = options.Value;
        _textAnalyticsClient = new TextAnalyticsClient(
            new Uri(config.Endpoint),
            new AzureKeyCredential(config.ApiKey));
    }

    public async Task<string> AbstractiveSummarizeAsync(string text, CancellationToken cancellationToken)
    {
        var documents = new List<string> { text };

        var operation = await _textAnalyticsClient.AbstractiveSummarizeAsync(
            WaitUntil.Completed,
            documents,
            cancellationToken: cancellationToken);

        await foreach (AbstractiveSummarizeResultCollection resultsPage in operation.Value)
        {
            foreach (AbstractiveSummarizeResult documentResult in resultsPage)
            {
                if (documentResult.HasError)
                {
                    throw new SummaryGeneralException(
                        "Azure Language Service returned an error.",
                        new { documentResult.Error.ErrorCode, documentResult.Error.Message });
                }

                return documentResult.Summaries.Any()
                    ? documentResult.Summaries.First().Text
                    : string.Empty;
            }
        }

        return string.Empty;
    }
}
