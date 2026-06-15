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
    private readonly AzureLanguageServiceConfigurations _config;

    public AzureLanguageServiceCaller(
        TextAnalyticsClient textAnalyticsClient,
        ILogger<AzureLanguageServiceCaller> logger,
        IOptions<AzureLanguageServiceConfigurations> options)
    {
        _textAnalyticsClient = textAnalyticsClient;
        _logger = logger;
        _config = options.Value;
    }

    public async Task<string> AbstractiveSummarizeAsync(string text, CancellationToken cancellationToken)
    {
        var chunks = TextChunker.Split(text, _config.MaxDocumentCharacterLength);

        var semaphore = new SemaphoreSlim(_config.MaxDegreeOfParallelism);
        var tasks = chunks.Select(async (chunk, index) =>
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                return (index, summary: await SummarizeDocumentAsync(chunk, cancellationToken));
            }
            finally
            {
                semaphore.Release();
            }
        });

        var results = await Task.WhenAll(tasks);

        return string.Join(" ", results.OrderBy(r => r.index).Select(r => r.summary));
    }

    private async Task<string> SummarizeDocumentAsync(string document, CancellationToken cancellationToken)
    {
        var operation = await _textAnalyticsClient.AbstractiveSummarizeAsync(
            WaitUntil.Completed,
            [document],
            cancellationToken: cancellationToken);

        await foreach (AbstractiveSummarizeResultCollection resultsPage in operation.Value)
        {
            foreach (AbstractiveSummarizeResult documentResult in resultsPage)
            {
                if (documentResult.HasError)
                {
                    _logger.LogError(
                        "Azure Language Service returned an error for a document. ErrorCode: {ErrorCode}, Message: {Message}",
                        documentResult.Error.ErrorCode,
                        documentResult.Error.Message);

                    throw new SummaryGeneralException("Azure Language Service returned an error.", debugDetails: new { documentResult.Error.ErrorCode, documentResult.Error.Message });
                }

                return documentResult.Summaries.Any()
                    ? documentResult.Summaries.First().Text
                    : string.Empty;
            }
        }

        return string.Empty;
    }
}
