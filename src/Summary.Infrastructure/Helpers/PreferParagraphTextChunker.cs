namespace Summary.Infrastructure.Helpers;

internal static class PreferParagraphTextChunker
{
    /// <summary>
    /// Splits <paramref name="text"/> into chunks that do not exceed
    /// <paramref name="maxChunkLength"/> characters, preferring to break
    /// on paragraph boundaries (\n\n), then sentence boundaries (.), then words.
    /// </summary>
    internal static IEnumerable<string> Split(string text, int maxChunkLength)
    {
        if (text.Length <= maxChunkLength)
            return [text];

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

            var breakAt = slice.LastIndexOf('\n');

            if (breakAt <= 0)
            {
                breakAt = slice.LastIndexOf('.');

                // Increment to include dot in the chunk
                if (breakAt > 0)
                    breakAt++;
            }

            if (breakAt <= 0)
                breakAt = slice.LastIndexOfAny([' ', '\t']);

            var chunkLength = breakAt > 0 ? breakAt : maxChunkLength;

            chunks.Add(remaining[..chunkLength].TrimEnd().ToString());
            remaining = remaining[chunkLength..].TrimStart();
        }

        return chunks;
    }
}
