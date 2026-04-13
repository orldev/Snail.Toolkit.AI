using System.Text.Json.Serialization;
using Snail.Toolkit.AI.Contracts.Schema;

namespace Snail.Toolkit.AI.Contracts.Responses;
/// <summary>
/// Provides a common base for model responses, containing shared metadata and performance metrics.
/// </summary>
internal abstract record ResponseBase
{
    /// <summary>
    /// Gets the identifier of the model used to generate the response.
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; init; } = string.Empty;

    /// <summary>
    /// Gets the timestamp when the response was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>
    /// Gets a value indicating whether the generation process has completed.
    /// </summary>
    [JsonPropertyName("done")]
    public bool Done { get; init; }

    /// <summary>
    /// Gets the reason the model stopped generating (e.g., "stop", "length").
    /// </summary>
    [JsonPropertyName("done_reason")]
    public string? DoneReason { get; init; }

    /// <summary>
    /// Gets the total time spent processing the request, in nanoseconds.
    /// </summary>
    [JsonPropertyName("total_duration")]
    public long? TotalDuration { get; init; }

    /// <summary>
    /// Gets the time spent loading the model, in nanoseconds.
    /// </summary>
    [JsonPropertyName("load_duration")]
    public long? LoadDuration { get; init; }

    /// <summary>
    /// Gets the number of tokens in the prompt that were evaluated.
    /// </summary>
    [JsonPropertyName("prompt_eval_count")]
    public int? PromptEvalCount { get; init; }

    /// <summary>
    /// Gets the time spent evaluating the prompt, in nanoseconds.
    /// </summary>
    [JsonPropertyName("prompt_eval_duration")]
    public long? PromptEvalDuration { get; init; }

    /// <summary>
    /// Gets the number of tokens generated in the response.
    /// </summary>
    [JsonPropertyName("eval_count")]
    public int? EvalCount { get; init; }

    /// <summary>
    /// Gets the time spent generating the response tokens, in nanoseconds.
    /// </summary>
    [JsonPropertyName("eval_duration")]
    public long? EvalDuration { get; init; }

    /// <summary>
    /// Gets the detailed log probabilities for the generated tokens, if requested.
    /// </summary>
    [JsonPropertyName("logprobs")]
    public IEnumerable<LogProbResult>? LogProbs { get; init; }
}