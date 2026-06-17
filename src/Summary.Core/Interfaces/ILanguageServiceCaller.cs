namespace Summary.Core.Interfaces;

public interface ILanguageServiceCaller
{
    Task<string> AbstractiveSummarizeAsync(string text, CancellationToken cancellationToken);
}
