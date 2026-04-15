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
/// <param name="ToolCalls">
/// An optional collection of tool/function calls that the assistant wants to invoke. 
/// Typically used when the assistant's role requests to execute one or more functions. 
/// Defaults to null.
/// </param>
/// <param name="ToolCallId">
/// An optional identifier that links a tool response message back to a specific tool call. 
/// Required when role is "tool" to indicate which tool call this message is responding to. 
/// Defaults to null.
/// </param>
public record ChatMessage(
    [property: JsonPropertyName("role")] string Role,
    [property: JsonPropertyName("content")] string Content,
    [property: JsonPropertyName("images")] IEnumerable<string>? Images = null,
    [property: JsonPropertyName("tool_calls")] IEnumerable<ToolCall>? ToolCalls = null,
    [property: JsonPropertyName("tool_call_id")] string? ToolCallId = null
);