using Snail.Toolkit.AI.Contracts.Requests;
using Snail.Toolkit.AI.Contracts.Responses;

namespace Snail.Toolkit.AI.Abstractions;

/// <summary>
/// Defines a contract for generating high-dimensional vector representations (embeddings) from text.
/// </summary>
public interface IEmbeddingsClient
{
    /// <summary>
    /// Generates embeddings for the provided input strings in a single asynchronous operation.
    /// </summary>
    /// <param name="request">The request containing the model identifier and the input strings to vectorize.</param>
    /// <param name="cancellationToken">A token to cancel the embedding generation request.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the <see cref="EmbeddingsResponse"/> 
    /// with the generated vectors.
    /// </returns>
    /// <remarks>
    /// Embeddings are typically used for semantic search, clustering, or as inputs for downstream machine learning tasks.
    /// Unlike chat or generation, this process does not support streaming.
    /// </remarks>
    Task<EmbeddingsResponse> GenerateAsync(EmbeddingsRequest request, CancellationToken cancellationToken = default);
}