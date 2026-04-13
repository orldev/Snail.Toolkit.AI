using System.Text.Json.Serialization;
using Snail.Toolkit.AI.Contracts.Schema;

namespace Snail.Toolkit.AI.Contracts.Requests;

/// <summary>
/// Provides a common base for all model requests, containing core configuration and generation settings.
/// </summary>
/// <param name="Model">The identifier of the model to be used for the request.</param>
public abstract record RequestBase(string Model)
{
    /// <summary>
    /// Gets or sets the identifier of the model.
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; } = Model;

    /// <summary>
    /// Gets or sets a value indicating whether the response should be streamed as partial data chunks.
    /// Defaults to <see langword="true"/>.
    /// </summary>
    [JsonPropertyName("stream")]
    public bool Stream { get; set; } = true;

    /// <summary>
    /// Gets or sets the format of the output. 
    /// Can be a <see cref="string"/> (e.g., "json") or a JSON Schema object for structured output.
    /// </summary>
    [JsonPropertyName("format")]
    public object? Format { get; set; }

    /// <summary>
    /// Gets or sets the advanced model parameters such as temperature, seed, and token limits.
    /// </summary>
    [JsonPropertyName("options")]
    public ModelOptions? Options { get; set; }

    /// <summary>
    /// Gets or sets the duration the model should stay loaded in memory after the request.
    /// Typically a duration string (e.g., "5m", "10s") or a number of seconds. Defaults to "5m".
    /// </summary>
    [JsonPropertyName("keep_alive")] 
    public string? KeepAlive { get; set; } = "5m";

    /// <summary>
    /// Gets or sets the specific instructions or parameters for models that support deep reasoning/thinking capabilities.
    /// </summary>
    [JsonPropertyName("think")]
    public string? Think { get; set; }
}