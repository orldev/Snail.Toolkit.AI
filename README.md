# Snail.Toolkit.AI

A lightweight, high-performance C# wrapper for interacting with AI models (optimized for Ollama and similar Line-Delimited JSON APIs). This library provides a unified, type-safe interface for **Chat**, **Text Generation**, and **Embeddings**.

## Features

- **Unified Entry Point:** Access all features through a single `IAiClient` gateway.
- **Streaming Support:** Real-time token streaming using `IAsyncEnumerable`.
- **Advanced Buffering:** Built-in extensions to buffer streams into readable chunks for UIs.
- **Strictly Typed:** Full support for JSON Schemas, tool calling, and multimodal inputs (images).
- **Extensible:** Built on standard `HttpClient` and `System.Text.Json`.

## Installation

Register the client in your dependency injection container:

```csharp
services.AddHttpClient<IAiClient, AiClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:11434");
});
```

## Basic Usage

### 1. The Gateway (`IAiClient`)
The `AiClient` is the primary entry point. It categorizes operations into three specialized clients:

```csharp
public interface IAiClient
{
    IChatClient Chats { get; }           // Conversational AI
    IEmbeddingsClient Embeddings { get; } // Vector Generation
    IGenerateClient Generate { get; }    // Simple Completions
}
```

---

### 2. Chat (Conversational)
Use `Chats` for multi-turn dialogues. It maintains roles (System, User, Assistant) and supports tool calling.

```csharp
var request = new ChatRequest("llama3", new[]
{
    new ChatMessage("system", "You are a helpful assistant."),
    new ChatMessage("user", "What is the capital of France?")
});

await foreach (var chunk in aiClient.Chats.StreamAsync(request))
{
    Console.Write(chunk.Content);
}
```

---

### 3. Text Generation (Completions)
Use `Generate` for one-off prompts, creative writing, or image analysis (multimodal).

```csharp
var request = new GenerateRequest("llava", "Describe this image.")
{
    Images = new[] { "base64_encoded_image_data..." }
};

await foreach (var chunk in aiClient.Generate.StreamAsync(request))
{
    Console.Write(chunk.Content);
}
```

---

### 4. Vector Embeddings
Use `Embeddings` to convert text into numerical vectors for semantic search or RAG (Retrieval-Augmented Generation).

```csharp
var request = new EmbeddingsRequest("mxbai-embed-large", new[] { 
    "The cat is on the mat", 
    "A feline is resting on a rug" 
});

var response = await aiClient.Embeddings.GenerateAsync(request);

foreach (var vector in response.Embeddings)
{
    // vector is a float[]
    Console.WriteLine($"Dimension: {vector.Length}");
}
```

## Advanced Streaming & Buffering

Sometimes, processing every single token is too "chatty" for your UI. Use `ExecuteBufferedAsync` to group tokens into larger character segments.

```csharp
// Yields strings only when they reach 50 characters or the stream ends
await foreach (string block in aiClient.Chats.ExecuteBufferedAsync(request, chunkSize: 50))
{
    Console.WriteLine($"--- New Block ---\n{block}");
}
```

## Configuration Options

Fine-tune the model behavior using `ModelOptions`:

```csharp
var request = new ChatRequest("llama3", messages)
{
    Options = new ModelOptions
    {
        Temperature = 0.7f,
        TopK = 40,
        Seed = 42
    },
    KeepAlive = "10m" // Keep model in memory for 10 minutes
};
```

## Technical Overview

| Feature | Interface | Request Type | Response Type |
| :--- | :--- | :--- | :--- |
| **Chat** | `IChatClient` | `ChatRequest` | `IAsyncEnumerable<StreamChunk>` |
| **Completion** | `IGenerateClient` | `GenerateRequest` | `IAsyncEnumerable<StreamChunk>` |
| **Embeddings** | `IEmbeddingsClient` | `EmbeddingsRequest` | `Task<EmbeddingsResponse>` |

## Error Handling

The library utilizes `EnsureSuccessStatusCode`. If the API returns an error (e.g., Model Not Found), an `HttpRequestException` will be thrown. If the JSON is malformed, an `InvalidOperationException` is raised.

```csharp
try 
{
    await foreach (var chunk in aiClient.Generate.StreamAsync(request)) { ... }
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"API Error: {ex.Message}");
}
```

## License

Snail.Toolkit.AI is a free and open source project, released under the permissible [MIT license](LICENSE).
