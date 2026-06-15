using Summary.Core.Models;

namespace Summary.Core.Interfaces;

public interface ISummarizeService
{
    Task<SummarizeResponse> SummarizeAsync(SummarizeRequest request, CancellationToken cancellationToken);
}
