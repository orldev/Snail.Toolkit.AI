using Snail.Toolkit.AI.Contracts.Schema;

namespace Snail.Toolkit.AI.Abstractions;

/// <summary>
/// Defines a contract for AI clients capable of returning responses as an asynchronous stream.
/// </summary>
/// <typeparam name="TRequest">The type of request object supported by the client.</typeparam>
public interface IStreamingAiClient<in TRequest>
{
    /// <summary>
    /// Initiates a streaming request to the AI model.
    /// </summary>
    /// <param name="request">The request parameters tailored to the specific client implementation.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{StreamChunk}"/> providing real-time access to the model's output as it is generated.
    /// </returns>
    IAsyncEnumerable<StreamChunk> StreamAsync(TRequest request, CancellationToken cancellationToken = default);
}