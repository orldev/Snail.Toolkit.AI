using Snail.Toolkit.AI.Contracts.Requests;

namespace Snail.Toolkit.AI.Abstractions;

/// <summary>
/// Defines a client specialized for conversational AI interactions.
/// </summary>
/// <remarks>
/// This interface inherits from <see cref="IStreamingAiClient{ChatRequest}"/>. 
/// It is designed for multi-turn dialogues where message history, roles (system, user, assistant), 
/// and potential tool usage are required to maintain context.
/// </remarks>
public interface IChatClient : IStreamingAiClient<ChatRequest>, Microsoft.Extensions.AI.IChatClient;