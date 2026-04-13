using System.Text.Json.Serialization;
using Snail.Toolkit.AI.Contracts.Schema;

namespace Snail.Toolkit.AI.Contracts.Requests;

/// <summary>
/// Represents a request for a structured chat conversation with a model.
/// </summary>
/// <param name="Model">The identifier of the model to be used for the chat.</param>
/// <param name="Messages">The history of the conversation to provide context for the model's response.</param>
public record ChatRequest(string Model, IEnumerable<ChatMessage> Messages) : RequestBase(Model)
{
    /// <summary>
    /// Gets or sets the collection of messages that make up the conversation thread.
    /// </summary>
    [JsonPropertyName("messages")]
    public IEnumerable<ChatMessage>? Messages { get; set; } = Messages;

    /// <summary>
    /// Gets or sets a list of tools (functions) the model may call during the conversation.
    /// Usually provided as a collection of function definitions or JSON schemas.
    /// </summary>
    [JsonPropertyName("tools")]
    public IEnumerable<object>? Tools { get; set; }
}