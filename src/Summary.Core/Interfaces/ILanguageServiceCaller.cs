namespace Summary.Core.Interfaces;

public interface ILanguageServiceCaller
{
    Task<IEnumerable<string>> AbstractiveSummarizeAsync(string text, CancellationToken cancellationToken);
}
