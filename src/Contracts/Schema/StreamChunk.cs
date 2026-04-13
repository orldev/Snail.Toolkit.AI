namespace Snail.Toolkit.AI.Contracts.Schema;

/// <summary>
/// Represents an individual data chunk received during a streamed model response.
/// </summary>
/// <param name="Model">The identifier of the model generating the response.</param>
/// <param name="Content">The partial text content or delta transmitted in this specific chunk.</param>
/// <param name="IsDone">A flag indicating whether the stream has reached its conclusion.</param>
public record StreamChunk(
    string Model,
    string Content,
    bool IsDone);