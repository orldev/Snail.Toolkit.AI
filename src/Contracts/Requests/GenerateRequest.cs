using System.Text.Json.Serialization;

namespace Snail.Toolkit.AI.Contracts.Requests;

/// <summary>
/// Represents a request for a basic text completion or generation from a model.
/// </summary>
/// <param name="Model">The identifier of the model to use for generation.</param>
/// <param name="Prompt">The input text to provide to the model for completion.</param>
public record GenerateRequest(string Model, string Prompt) : RequestBase(Model)
{
    /// <summary>
    /// Gets or sets the main text prompt for the model to process.
    /// </summary>
    [JsonPropertyName("prompt")]
    public string? Prompt { get; set; } = Prompt;

    /// <summary>
    /// Gets or sets an optional collection of Base64-encoded image strings for multimodal processing.
    /// </summary>
    [JsonPropertyName("images")]
    public IEnumerable<string>? Images { get; set; }
}