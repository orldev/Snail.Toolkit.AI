using System.Text.Json;
using Microsoft.Extensions.AI;

namespace Snail.Toolkit.AI.Contracts.Mapping;

/// <summary>
/// Provides extension methods for mapping between chat-related domain models and internal API models.
/// </summary>
internal static class ChatMappingExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    
    /// <summary>
    /// Converts a collection of chat messages along with options and model information into an internal chat request.
    /// </summary>
    /// <param name="messages">The collection of chat messages to be sent.</param>
    /// <param name="options">Optional chat configuration parameters such as temperature, tokens, etc.</param>
    /// <param name="model">The model identifier to be used for the chat completion.</param>
    /// <returns>An internal chat request object ready for API consumption.</returns>
    public static Requests.ChatRequest ToInternalRequest(
        this IEnumerable<ChatMessage> messages, 
        ChatOptions? options, 
        string model)
    {
        return new Requests.ChatRequest(model, messages.Select(m => m.ToInternalMessage()))
        {
            Options = options?.ToInternalOptions(),
            Tools = options?.Tools?.OfType<AIFunction>().Select(f => f.ToInternalTool())
        };
    }

    /// <summary>
    /// Converts a domain chat message to an internal schema message, handling role determination
    /// and content mapping for function calls and results.
    /// </summary>
    /// <param name="message">The chat message to convert.</param>
    /// <returns>An internal schema chat message with properly mapped role, content, and tool calls.</returns>
    /// <remarks>
    /// This method processes the message contents to:
    /// - Set role to "tool" for function results
    /// - Set role to "assistant" for function calls
    /// - Extract text content, function call IDs, and arguments
    /// </remarks>
    private static Schema.ChatMessage ToInternalMessage(this ChatMessage message)
    {
        var role = message.Role.Value.ToLowerInvariant();
        var content = message.Text;
        string? toolCallId = null;
        List<Schema.ToolCall>? toolCalls = null;
        
        foreach (var item in message.Contents)
        {
            switch (item)
            {
                case FunctionResultContent result:
                    role = "tool";
                    content = result.Result?.ToString();
                    toolCallId = result.CallId;
                    break;

                case FunctionCallContent call:
                    toolCalls ??= [];
                    toolCalls.Add(new Schema.ToolCall(
                        Id: call.CallId,
                        Type: "function",
                        Function: new Schema.FunctionCall(
                            Name: call.Name,
                            Arguments: JsonSerializer.SerializeToElement(call.Arguments, JsonOptions)
                        )
                    ));
                    break;
                
                case TextContent text:
                    content = text.Text;
                    break;
            }
        }
        
        if (toolCalls?.Count > 0)
        {
            role = "assistant";
        }

        return new Schema.ChatMessage(
            Role: role,
            Content: content ?? string.Empty,
            ToolCallId: toolCallId,
            ToolCalls: toolCalls
        );
    }

    /// <summary>
    /// Converts chat options to internal model options for API consumption.
    /// </summary>
    /// <param name="options">The chat options to convert.</param>
    /// <returns>Internal model options with mapped temperature, max tokens, top-p, and top-k values.</returns>
    private static Schema.ModelOptions ToInternalOptions(this ChatOptions options) => new()
    {
        Temperature = options.Temperature,
        MaxTokens = options.MaxOutputTokens,
        TopP = options.TopP,
        TopK = options.TopK
    };

    /// <summary>
    /// Converts an AI function to an internal tool representation for API requests.
    /// </summary>
    /// <param name="function">The AI function to convert.</param>
    /// <returns>An anonymous object representing the function as a tool with type, name, description, and JSON schema.</returns>
    private static object ToInternalTool(this AIFunction function) => new
    {
        type = "function",
        function = new
        {
            name = function.Name,
            description = function.Description,
            parameters = function.JsonSchema
        }
    };
    
    /// <summary>
    /// Converts an internal API response to a domain chat response model.
    /// </summary>
    /// <param name="response">The internal response from the chat API.</param>
    /// <returns>A domain chat response containing the assistant's message and metadata.</returns>
    public static ChatResponse ToAiResponse(this Responses.ChatResponse response)
    {
        var aiMessage = new ChatMessage(
            new ChatRole(response.Message.Role),
            response.Message.Content
        );

        MapToolCallsToContents(response.Message.ToolCalls, aiMessage.Contents);

        return new ChatResponse(aiMessage)
        {
            ModelId = response.Model,
            ResponseId = Guid.NewGuid().ToString()
        };
    }
    
    /// <summary>
    /// Converts an internal API response to a streaming update representation.
    /// </summary>
    /// <param name="response">The internal response from the chat API.</param>
    /// <returns>A chat response update suitable for streaming scenarios, containing incremental content.</returns>
    public static ChatResponseUpdate ToAiUpdate(this Responses.ChatResponse response)
    {
        var update = new ChatResponseUpdate
        {
            Role = new ChatRole(response.Message.Role),
            RawRepresentation = response
        };

        if (!string.IsNullOrEmpty(response.Message.Content))
        {
            update.Contents.Add(new TextContent(response.Message.Content));
        }

        MapToolCallsToContents(response.Message.ToolCalls, update.Contents);

        return update;
    }
    
    /// <summary>
    /// Maps internal tool calls to domain function call contents.
    /// </summary>
    /// <param name="internalToolCalls">The collection of internal tool calls from the API response.</param>
    /// <param name="targetContents">The target content collection to populate with function call contents.</param>
    /// <remarks>
    /// This method deserializes function arguments from JSON elements into dictionaries
    /// and adds them as FunctionCallContent objects to the target collection.
    /// </remarks>
    private static void MapToolCallsToContents(IEnumerable<Schema.ToolCall>? internalToolCalls, IList<AIContent> targetContents)
    {
        if (internalToolCalls == null) return;

        foreach (var tc in internalToolCalls)
        {
            var args = tc.Function.Arguments.ValueKind != JsonValueKind.Undefined 
                ? tc.Function.Arguments.Deserialize<Dictionary<string, object?>>(JsonOptions) 
                : null;

            targetContents.Add(new FunctionCallContent(
                callId: tc.Id,
                name: tc.Function.Name,
                arguments: args
            ));
        }
    }
}