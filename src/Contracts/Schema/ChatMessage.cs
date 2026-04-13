using System.Text.Json.Serialization;

namespace Snail.Toolkit.AI.Contracts.Schema;

/// <summary>
/// Represents a message within a chat conversation, including the sender's role and the message content.
/// </summary>
/// <param name="Role">The role of the message sender, such as "system", "user", or "assistant".</param>
/// <param name="Content">The textual content of the message.</param>
/// <param name="Images">
/// An optional collection of images associated with the message, typically provided as Base64 encoded strings or URLs. 
/// Defaults to null.
/// </param>
public record ChatMessage(
    [property: JsonPropertyName("role")] string Role,
    [property: JsonPropertyName("content")] string Content,
    [property: JsonPropertyName("images")] IEnumerable<string>? Images = null
);