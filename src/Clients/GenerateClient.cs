using Snail.Toolkit.AI.Abstractions;
using Snail.Toolkit.AI.Clients.Extensions;
using Snail.Toolkit.AI.Contracts.Requests;
using Snail.Toolkit.AI.Contracts.Responses;
using Snail.Toolkit.AI.Contracts.Schema;

namespace Snail.Toolkit.AI.Clients;

/// <summary>
/// A client dedicated to handling basic text generation and completion requests.
/// </summary>
/// <param name="httpClient">The <see cref="HttpClient"/> instance used to perform API requests.</param>
public class GenerateClient(HttpClient httpClient) : IGenerateClient
{
    private const string ChatEndpoint = "/api/generate";

    /// <summary>
    /// Sends a generation request and returns an asynchronous stream of response chunks.
    /// </summary>
    /// <param name="request">The parameters for the generation task, including the prompt and model options.</param>
    /// <param name="cancellationToken">A token to cancel the streaming operation.</param>
    /// <returns>An <see cref="IAsyncEnumerable{StreamChunk}"/> that yields partial response data as it arrives.</returns>
    /// <remarks>
    /// This method maps the internal <see cref="GenerateResponse"/> into a flattened <see cref="StreamChunk"/> 
    /// for a consistent consumption experience across different client types.
    /// </remarks>
    public IAsyncEnumerable<StreamChunk> StreamAsync(
        GenerateRequest request,
        CancellationToken cancellationToken = default)
    {
        return httpClient.ReadAsStreamAsync<GenerateRequest, GenerateResponse, StreamChunk>(
            endpoint: ChatEndpoint,
            request: request,
            selector: chunk => new StreamChunk(chunk.Model, chunk.Response, chunk.Done),
            isDoneSelector: chunk => chunk.Done,
            cancellationToken: cancellationToken
        );
    }
}