using System.Text.Json.Serialization;

namespace Snail.Toolkit.AI.Contracts.Schema;

/// <summary>
/// Represents a specific token and its associated log probability.
/// </summary>
/// <param name="Token">The textual representation of the token.</param>
/// <param name="Logprob">The log probability of the token occurring.</param>
/// <param name="Bytes">
/// A list of integers representing the UTF-8 bytes for the token. 
/// This can be null if the token does not have a direct byte representation.
/// </param>
internal record TopLogProb(
    [property: JsonPropertyName("token")] string Token,
    [property: JsonPropertyName("logprob")] double Logprob,
    [property: JsonPropertyName("bytes")] IEnumerable<int>? Bytes
);