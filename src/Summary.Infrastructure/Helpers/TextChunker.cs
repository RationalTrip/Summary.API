namespace Summary.Infrastructure.Helpers;

internal static class TextChunker
{
    /// <summary>
    /// Splits <paramref name="text"/> into chunks that do not exceed
    /// <paramref name="maxChunkLength"/> characters, breaking only on whitespace.
    /// </summary>
    internal static IReadOnlyList<string> Split(string text, int maxChunkLength)
    {
        var chunks = new List<string>();
        var remaining = text.AsSpan().Trim();

        while (!remaining.IsEmpty)
        {
            if (remaining.Length <= maxChunkLength)
            {
                chunks.Add(remaining.ToString());
                break;
            }

            var slice = remaining[..maxChunkLength];
            var breakAt = slice.LastIndexOfAny([' ', '\n', '\r', '\t']);

            var chunkLength = breakAt > 0 ? breakAt : maxChunkLength;

            chunks.Add(remaining[..chunkLength].ToString());
            remaining = remaining[chunkLength..].TrimStart();
        }

        return chunks;
    }
}
