using System.Text.Json;
using System.Text.Json.Serialization;

namespace Snail.Toolkit.AI.Contracts.Schema;

/// <summary>
/// Represents the details of a function to be called, as determined by the model.
/// </summary>
/// <param name="Name">The name of the function to be called.</param>
/// <param name="Description">An optional description of what the function does or why it is being called.</param>
/// <param name="Arguments">
/// The raw JSON arguments for the function call. 
/// These are typically stored as a <see cref="JsonElement"/> and should be deserialized into the expected parameter type.
/// </param>
public record FunctionCall(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("arguments")] JsonElement Arguments,
    [property: JsonPropertyName("description")] string? Description = null);