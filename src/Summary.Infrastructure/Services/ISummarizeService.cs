using Summary.Application.Models;

namespace Summary.Infrastructure.Services;

public interface ISummarizeService
{
    Task<SummarizeResponse> SummarizeAsync(SummarizeRequest request, CancellationToken cancellationToken);
}
