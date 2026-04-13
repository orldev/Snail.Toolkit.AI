using System.Text.Json.Serialization;

namespace Snail.Toolkit.AI.Contracts.Requests;

/// <summary>
/// Represents a request to generate vector embeddings for one or more input strings.
/// </summary>
/// <param name="Model">The identifier of the model to use for generating embeddings.</param>
/// <param name="Input">
/// A collection of text strings to be converted into embeddings. 
/// Passing multiple strings allows for efficient batch processing.
/// </param>
/// <param name="Options">
/// Additional model-specific parameters to control the embedding process. 
/// Defaults to <see langword="null"/>.
/// </param>
public record EmbeddingsRequest(
    [property: JsonPropertyName("model")] string Model,
    [property: JsonPropertyName("input")] IEnumerable<string> Input,
    [property: JsonPropertyName("options")] object? Options = null);