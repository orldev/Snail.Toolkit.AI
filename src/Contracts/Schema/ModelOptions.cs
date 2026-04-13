using System.Text.Json.Serialization;

namespace Snail.Toolkit.AI.Contracts.Schema;

/// <summary>
/// Configuration parameters to tune the model's generation behavior.
/// </summary>
public record ModelOptions
{
    /// <summary>
    /// Controls the randomness of the output. Higher values make the output more random, 
    /// while lower values make it more deterministic.
    /// </summary>
    [JsonPropertyName("temperature")] 
    public float? Temperature { get; init; }

    /// <summary>
    /// An alternative to sampling with temperature, called nucleus sampling, where the 
    /// model considers the results of the tokens with top_p probability mass.
    /// </summary>
    [JsonPropertyName("top_p")] 
    public float? TopP { get; init; }

    /// <summary>
    /// If specified, the system will make a best effort to sample deterministically, 
    /// such that repeated requests with the same seed and parameters should return the same result.
    /// </summary>
    [JsonPropertyName("seed")] 
    public int? Seed { get; init; }

    /// <summary>
    /// The maximum number of tokens to generate in the response. 
    /// Corresponds to "num_predict" in the underlying API.
    /// </summary>
    [JsonPropertyName("num_predict")] 
    public int? MaxTokens { get; init; }

    /// <summary>
    /// Reduces the probability of generating low-probability tokens by only 
    /// considering the top K most likely tokens.
    /// </summary>
    [JsonPropertyName("top_k")] 
    public int? TopK { get; init; }
}