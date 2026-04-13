using Snail.Toolkit.AI.Abstractions;

namespace Snail.Toolkit.AI.Clients;

/// <summary>
/// The primary entry point for interacting with the AI service. 
/// Provides unified access to chat, embedding, and generation capabilities.
/// </summary>
/// <param name="chatClient">The client responsible for conversational interactions.</param>
/// <param name="embeddingsClient">The client responsible for vectorizing text.</param>
/// <param name="generate">The client responsible for basic text completion.</param>
public class AiClient(
    IChatClient chatClient,
    IEmbeddingsClient embeddingsClient,
    IGenerateClient generate)
    : IAiClient
{
    /// <summary>
    /// Gets the client for conversational chat interfaces.
    /// </summary>
    public IChatClient Chats { get; } = chatClient;
    
    /// <summary>
    /// Gets the client for generating vector embeddings.
    /// </summary>
    public IEmbeddingsClient Embeddings { get; } = embeddingsClient;

    /// <summary>
    /// Gets the client for standard text generation and completions.
    /// </summary>
    public IGenerateClient Generate { get; } = generate;
}