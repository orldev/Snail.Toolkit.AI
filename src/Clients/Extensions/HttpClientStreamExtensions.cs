using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Snail.Toolkit.AI.Clients.Extensions;

/// <summary>
/// Provides high-level extensions for <see cref="HttpClient"/> to handle line-delimited JSON streaming.
/// </summary>
public static class HttpClientStreamExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    /// <summary>
    /// Sends a POST request and streams the response lines, deserializing each line as a JSON object.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request body.</typeparam>
    /// <typeparam name="TResponse">The type of the raw JSON chunk received from the stream.</typeparam>
    /// <typeparam name="TResult">The type of the projected result returned to the caller.</typeparam>
    /// <param name="client">The <see cref="HttpClient"/> instance.</param>
    /// <param name="endpoint">The relative or absolute URL for the API endpoint.</param>
    /// <param name="request">The request payload to be serialized as JSON.</param>
    /// <param name="selector">A transformation function to convert the raw <typeparamref name="TResponse"/> into <typeparamref name="TResult"/>.</param>
    /// <param name="isDoneSelector">A predicate used to determine if the model has signaled the end of the stream.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="IAsyncEnumerable{TResult}"/> that yields projected chunks as they are received and parsed.</returns>
    /// <remarks>
    /// This method uses <see cref="HttpCompletionOption.ResponseHeadersRead"/> to begin processing the stream 
    /// immediately before the entire payload has been downloaded.
    /// </remarks>
    public static async IAsyncEnumerable<TResult> ReadAsStreamAsync<TRequest, TResponse, TResult>(
        this HttpClient client,
        string endpoint,
        TRequest request,
        Func<TResponse, TResult> selector,
        Func<TResponse, bool> isDoneSelector,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint);
        httpRequest.Content = JsonContent.Create(request);

        // Start reading as soon as headers are available to enable real-time streaming
        using var response = await client.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        using var reader = new StreamReader(stream, Encoding.UTF8);

        while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(line)) 
                continue;

            var chunk = JsonSerializer.Deserialize<TResponse>(line, JsonOptions);
            if (chunk is null) 
                continue;

            yield return selector(chunk);

            // Break the loop if the model metadata indicates completion
            if (isDoneSelector(chunk)) 
                break;
        }
    }
}