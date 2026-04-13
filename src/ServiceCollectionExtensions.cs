using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Snail.Toolkit.AI.Abstractions;
using Snail.Toolkit.AI.Clients;
using Snail.Toolkit.AI.Configuration;

namespace Snail.Toolkit.AI;

/// <summary>
/// Provides extension methods for registering AI client services in the <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the core AI services, including Chat, Generation, and Embeddings clients.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configureOptions">An optional delegate to configure the <see cref="AiClientOptions"/>.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddAiCore(
        this IServiceCollection services, 
        Action<AiClientOptions>? configureOptions = null)
    {
        var optionsBuilder = services.AddOptions<AiClientOptions>();
        
        if (configureOptions != null)
        {
            optionsBuilder.Configure(configureOptions);
        }
        
        return services.AddAiInternal();
    }

    /// <summary>
    /// Registers the core AI services by binding configuration from the specified <see cref="IConfiguration"/> section.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configuration">The configuration section to bind (e.g., config.GetSection("AiClient")).</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddAiCore(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.Configure<AiClientOptions>(configuration);
        return services.AddAiInternal();
    }

    /// <summary>
    /// Internal helper to register HttpClients and the unified AiClient facade.
    /// </summary>
    private static IServiceCollection AddAiInternal(this IServiceCollection services)
    {
        // Common client configuration (BaseUrl, Timeout, Auth)
        Action<IServiceProvider, HttpClient> configureClient = (sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<AiClientOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = options.Timeout;
            
            if (!string.IsNullOrEmpty(options.ApiKey))
            {
                client.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", options.ApiKey);
            }
        };

        // Optimized handler for long-lived AI streaming connections
        var configureHandler = () => new SocketsHttpHandler
        {
            PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
            KeepAlivePingDelay = TimeSpan.FromSeconds(30),
            KeepAlivePingTimeout = TimeSpan.FromSeconds(10),
            EnableMultipleHttp2Connections = true
        };

        // Register individual specialized clients with their own HttpClients
        services.AddHttpClient<IChatClient, ChatClient>(configureClient)
            .ConfigurePrimaryHttpMessageHandler(configureHandler);

        services.AddHttpClient<IGenerateClient, GenerateClient>(configureClient)
            .ConfigurePrimaryHttpMessageHandler(configureHandler);

        services.AddHttpClient<IEmbeddingsClient, EmbeddingsClient>(configureClient)
            .ConfigurePrimaryHttpMessageHandler(configureHandler);

        // Register the main facade as Transient
        services.AddTransient<IAiClient, AiClient>();

        return services;
    }
}