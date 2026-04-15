using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using Snail.Toolkit.AI.Clients.Extensions;
using Snail.Toolkit.AI.Configuration;
using Snail.Toolkit.AI.Contracts.Mapping;
using Snail.Toolkit.AI.Contracts.Requests;
using Snail.Toolkit.AI.Contracts.Schema;

using ChatMessage = Microsoft.Extensions.AI.ChatMessage;
using ChatResponse = Snail.Toolkit.AI.Contracts.Responses.ChatResponse;
using IChatClient = Snail.Toolkit.AI.Abstractions.IChatClient;

namespace Snail.Toolkit.AI.Clients;

/// <summary>
/// A client dedicated to handling conversational chat interactions with a model.
/// </summary>
/// <param name="httpClient">The <see cref="HttpClient"/> instance used to perform API requests.</param>
public class ChatClient(HttpClient httpClient, IOptions<AiClientOptions> options) : IChatClient
{
    private readonly AiClientOptions _options = options.Value;
    
    private const string ChatEndpoint = "/api/chat";
    
    /// <summary>
    /// Sends a chat request and returns an asynchronous stream of conversation chunks.
    /// </summary>
    /// <param name="request">The chat parameters, including conversation history, tools, and model settings.</param>
    /// <param name="cancellationToken">A token to cancel the streaming operation.</param>
    /// <returns>An <see cref="IAsyncEnumerable{StreamChunk}"/> that yields partial message content as it is generated.</returns>
    /// <remarks>
    /// Unlike the generation client, this method specifically extracts text from the <see cref="Message.Content"/> 
    /// property within each response chunk to provide a streamlined output.
    /// </remarks>
    public IAsyncEnumerable<StreamChunk> StreamAsync(
        ChatRequest request, 
        CancellationToken cancellationToken = default)
    {
        return httpClient.ReadAsStreamAsync<ChatRequest, ChatResponse, StreamChunk>(
            endpoint: ChatEndpoint,
            request: request,
            selector: chunk => new StreamChunk(chunk.Model, chunk.Message.Content, chunk.Done),
            isDoneSelector: chunk => chunk.Done,
            cancellationToken: cancellationToken
        );
    }
    
    /// <summary>
    /// Sends a chat request with conversation history and returns a complete response.
    /// </summary>
    /// <param name="messages">The conversation history including system, user, and assistant messages.</param>
    /// <param name="options">Optional configuration parameters including model selection and generation settings.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A complete chat response containing the assistant's message and metadata.</returns>
    public async Task<Microsoft.Extensions.AI.ChatResponse> GetResponseAsync(
        IEnumerable<ChatMessage> messages, ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        
        var request = messages.ToInternalRequest(options, options?.ModelId ?? _options.DefaultModel ?? string.Empty);
        request.Stream = false;
        
        var response = await httpClient.PostAsJsonAsync(ChatEndpoint, request, jsonOptions, cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ChatResponse>(jsonOptions, cancellationToken);
        return result!.ToAiResponse();
        
    }

    /// <summary>
    /// Sends a chat request with conversation history and returns a streaming response.
    /// </summary>
    /// <param name="messages">The conversation history including system, user, and assistant messages.</param>
    /// <param name="options">Optional configuration parameters including generation settings.</param>
    /// <param name="cancellationToken">A token to cancel the streaming operation.</param>
    /// <returns>An asynchronous enumerable of chat response updates, yielding partial content as it becomes available.</returns>
    public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
        IEnumerable<ChatMessage> messages, ChatOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var request = messages.ToInternalRequest(options, options?.ModelId ?? _options.DefaultModel ?? string.Empty);
        
        return httpClient.ReadAsStreamAsync<ChatRequest, ChatResponse, ChatResponseUpdate>(
            endpoint: ChatEndpoint,
            request: request,
            selector: chunk => chunk.ToAiUpdate(),
            isDoneSelector: chunk => chunk.Done,
            cancellationToken: cancellationToken
        );
    }

    /// <summary>
    /// Gets a service object of the specified type.
    /// </summary>
    /// <param name="serviceType">The type of service to retrieve.</param>
    /// <param name="serviceKey">An optional key for selecting the service implementation.</param>
    /// <returns>
    /// The service instance if this client implements the requested type; otherwise, null.
    /// </returns>
    public object? GetService(Type serviceType, object? serviceKey = null) =>
        serviceType.IsInstanceOfType(this) ? this : null;
    
    /// <summary>
    /// Disposes the underlying HTTP client, releasing all resources.
    /// </summary>
    public void Dispose() => httpClient.Dispose();
}