namespace Snail.Toolkit.AI.Abstractions;

/// <summary>
/// Defines the unified service interface for interacting with various AI model capabilities.
/// </summary>
/// <remarks>
/// This interface acts as a central hub, providing access to specialized clients for 
/// chat, embeddings, and basic text generation. It is designed to be the primary 
/// dependency injected into application services.
/// </remarks>
public interface IAiClient
{
    /// <summary>
    /// Gets the client specialized for conversational, multi-turn AI interactions.
    /// </summary>
    IChatClient Chats { get; }
    
    /// <summary>
    /// Gets the client specialized for generating vector embeddings from text.
    /// </summary>
    IEmbeddingsClient Embeddings { get; }
    
    /// <summary>
    /// Gets the client specialized for standard text completion and generation.
    /// </summary>
    IGenerateClient Generate { get; }
}