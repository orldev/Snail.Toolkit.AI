using Snail.Toolkit.AI.Abstractions;
using Snail.Toolkit.AI.Clients.Extensions;
using Snail.Toolkit.AI.Contracts.Requests;
using Snail.Toolkit.AI.Contracts.Responses;
using Snail.Toolkit.AI.Contracts.Schema;

namespace Snail.Toolkit.AI.Clients;

/// <summary>
/// A client dedicated to handling conversational chat interactions with a model.
/// </summary>
/// <param name="httpClient">The <see cref="HttpClient"/> instance used to perform API requests.</param>
public class ChatClient(HttpClient httpClient) : IChatClient
{
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
}