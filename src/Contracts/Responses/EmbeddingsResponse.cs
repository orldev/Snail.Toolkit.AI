using System.Text.Json.Serialization;

namespace Snail.Toolkit.AI.Contracts.Responses;

/// <summary>
/// Represents the response from an embedding request, containing the vector representations of the input text.
/// </summary>
/// <param name="Model">The identifier of the model used to generate the embeddings.</param>
/// <param name="Embeddings">
/// A two-dimensional array containing the generated embedding vectors. 
/// Each inner array represents a single vector corresponding to a segment of the input.
/// </param>
/// <param name="PromptEvalCount">
/// The number of tokens in the prompt that were processed to generate these embeddings.
/// </param>
public record EmbeddingsResponse(
    [property: JsonPropertyName("model")] string Model,
    [property: JsonPropertyName("embeddings")] float[][] Embeddings,
    [property: JsonPropertyName("prompt_eval_count")] int? PromptEvalCount);