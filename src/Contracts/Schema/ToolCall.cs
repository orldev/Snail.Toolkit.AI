using System.Text.Json.Serialization;

namespace Snail.Toolkit.AI.Contracts.Schema;

/// <summary>
/// Represents a specific tool call initiated by the model, typically containing function execution details.
/// </summary>
/// <param name="Function">The name and arguments of the function to be called.</param>
internal record ToolCall(
    [property: JsonPropertyName("function")] FunctionCall Function
);