using System.Net.Http.Json;
using Snail.Toolkit.AI.Abstractions;
using Snail.Toolkit.AI.Contracts.Requests;
using Snail.Toolkit.AI.Contracts.Responses;

namespace Snail.Toolkit.AI.Clients;

/// <summary>
/// A client dedicated to generating vector embeddings from text inputs.
/// </summary>
/// <param name="httpClient">The <see cref="HttpClient"/> instance used to perform API requests.</param>
public class EmbeddingsClient(HttpClient httpClient) : IEmbeddingsClient
{
    private const string EmbedEndpoint = "/api/embed";

    /// <summary>
    /// Sends a request to generate vector embeddings for the provided input text.
    /// </summary>
    /// <param name="request">The parameters for the embedding task, including model and input strings.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Task{EmbeddingsResponse}"/> containing the generated vectors and metadata.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the API response cannot be deserialized into a <see cref="EmbeddingsResponse"/>.</exception>
    /// <exception cref="HttpRequestException">Thrown when the API call returns a non-success status code.</exception>
    public async Task<EmbeddingsResponse> GenerateAsync(
        EmbeddingsRequest request, 
        CancellationToken cancellationToken = default)
    {
        // TODO: fix
        using var response = await httpClient.PostAsJsonAsync(EmbedEndpoint, request, cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<EmbeddingsResponse>(cancellationToken: cancellationToken)
            .ConfigureAwait(false) ?? throw new InvalidOperationException("Failed to deserialize Ollama embeddings response.");
    }
}