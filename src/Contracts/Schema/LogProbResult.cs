using System.Text.Json.Serialization;

namespace Snail.Toolkit.AI.Contracts.Schema;

/// <summary>
/// Contains the log probability details for a specific chosen token, 
/// along with a list of the most likely alternative tokens.
/// </summary>
/// <param name="Token">The textual representation of the chosen token.</param>
/// <param name="Logprob">The log probability of the chosen token.</param>
/// <param name="Bytes">
/// A list of integers representing the UTF-8 bytes for the token. 
/// Null if no byte-level representation is available.
/// </param>
/// <param name="TopLogprobs">
/// An optional collection of the most likely candidate tokens at this position 
/// and their respective log probabilities.
/// </param>
internal record LogProbResult(
    [property: JsonPropertyName("token")] string Token,
    [property: JsonPropertyName("logprob")] double Logprob,
    [property: JsonPropertyName("bytes")] IEnumerable<int>? Bytes,
    [property: JsonPropertyName("top_logprobs")] IEnumerable<TopLogProb>? TopLogprobs
);