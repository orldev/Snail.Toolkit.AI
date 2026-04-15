using System.Text.Json.Serialization;

namespace Snail.Toolkit.AI.Contracts.Schema;

/// <summary>
/// Represents a specific tool call initiated by the model, typically containing function execution details.
/// </summary>
/// <param name="Id">
/// A unique identifier for the tool call, used to correlate requests with responses. 
/// Required when responding to a tool call from the assistant.
/// </param>
/// <param name="Type">
/// The type of the tool being called. Typically set to "function" for function-calling scenarios.
/// </param>
/// <param name="Function">
/// The function call details, including the function name and the arguments to be passed to it.
/// </param>
public record ToolCall(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("function")] FunctionCall Function
);