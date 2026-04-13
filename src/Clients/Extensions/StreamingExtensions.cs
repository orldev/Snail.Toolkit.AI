using System.Runtime.CompilerServices;
using System.Text;
using Snail.Toolkit.AI.Abstractions;

namespace Snail.Toolkit.AI.Clients.Extensions;

/// <summary>
/// Provides extension methods for handling AI streams with advanced buffering and transformation logic.
/// </summary>
public static class StreamingExtensions
{
    /// <summary>
    /// Executes a streaming request and buffers the output into larger text segments based on a specified character count.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request being sent to the AI client.</typeparam>
    /// <param name="client">The streaming client instance.</param>
    /// <param name="request">The request parameters.</param>
    /// <param name="chunkSize">
    /// The minimum character length each yielded string must reach before being returned. 
    /// If <see langword="null"/>, the entire response is buffered and yielded as a single string at the end.
    /// </param>
    /// <param name="ct">A token to cancel the streaming and buffering operation.</param>
    /// <returns>An <see cref="IAsyncEnumerable{String}"/> yielding buffered text segments.</returns>
    /// <remarks>
    /// This is particularly useful for reducing UI refresh rates or optimizing write operations 
    /// when the model generates many small tokens in rapid succession.
    /// </remarks>
    public static async IAsyncEnumerable<string> ExecuteBufferedAsync<TRequest>(
        this IStreamingAiClient<TRequest> client,
        TRequest request,
        int? chunkSize = null,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        var buffer = new StringBuilder();

        await foreach (var chunk in client.StreamAsync(request, ct).ConfigureAwait(false))
        {
            if (string.IsNullOrEmpty(chunk.Content)) continue;

            buffer.Append(chunk.Content);
            
            if (chunkSize.HasValue && buffer.Length >= chunkSize.Value)
            {
                yield return buffer.ToString();
                buffer.Clear();
            }
        }
        
        if (buffer.Length > 0)
        {
            yield return buffer.ToString();
        }
    }
}