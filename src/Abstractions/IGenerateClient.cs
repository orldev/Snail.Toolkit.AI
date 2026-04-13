using Snail.Toolkit.AI.Contracts.Requests;

namespace Snail.Toolkit.AI.Abstractions;

/// <summary>
/// Defines a client specialized for text completion and generation tasks.
/// </summary>
/// <remarks>
/// This interface inherits from <see cref="IStreamingAiClient{GenerateRequest}"/>, 
/// providing a streamlined way to process basic prompts that do not require conversational history.
/// </remarks>
public interface IGenerateClient : IStreamingAiClient<GenerateRequest>;