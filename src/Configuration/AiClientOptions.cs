namespace Snail.Toolkit.AI.Configuration;

/// <summary>
/// Configuration options for the AI client, defining connection details and request behavior.
/// </summary>
public class AiClientOptions
{
    /// <summary>
    /// Gets or sets the base URL for the API service. 
    /// Defaults to the standard local Ollama endpoint: "http://localhost:11434".
    /// </summary>
    public string BaseUrl { get; set; } = "http://localhost:11434";

    /// <summary>
    /// Gets or sets the API key used for authentication. 
    /// For local instances like Ollama, this often defaults to "ollama" or can be left null.
    /// </summary>
    public string? ApiKey { get; set; } = "ollama";

    /// <summary>
    /// Gets or sets the maximum time to wait for a response before timing out. 
    /// This should be long enough to accommodate model generation times.
    /// Defaults to 100 seconds.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(100);
}