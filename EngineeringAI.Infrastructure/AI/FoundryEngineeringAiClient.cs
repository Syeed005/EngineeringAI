using Azure.AI.OpenAI;
using Azure.Identity;
using EngineeringAI.Application.Interfaces.AI;
using EngineeringAI.Application.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;

namespace EngineeringAI.Infrastructure.AI {
    public class FoundryEngineeringAiClient : IEngineeringAiClient {
        private readonly ChatClient _chatClient;
        private readonly ILogger<FoundryEngineeringAiClient> _logger;
        private readonly AiOptions _aiOptions;

        public FoundryEngineeringAiClient(IOptions<AiOptions> options, ILogger<FoundryEngineeringAiClient> logger) {
            _aiOptions = options.Value;

            var azureClient = new AzureOpenAIClient(new Uri(_aiOptions.Endpoint), new DefaultAzureCredential());

            _chatClient = azureClient.GetChatClient(_aiOptions.Deployment);
            _logger = logger;
        }

        public async Task<string> GenerateAsync(string systemPrompt, string userPrompt, CancellationToken cancellationToken = default) {
            var messages = new List<ChatMessage>{
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(userPrompt)
            };
            var startedAt = Stopwatch.GetTimestamp();
            var response = await _chatClient.CompleteChatAsync(messages, cancellationToken: cancellationToken);
            var elapsed = Stopwatch.GetElapsedTime(startedAt);

            _logger.LogInformation("AI request completed in {ElapsedMilliseconds} ms.", elapsed.TotalMilliseconds);
            return response.Value.Content[0].Text;

        }

        public async Task<T> GenerateStructuredAsync<T>(string systemPrompt, string userPrompt, CancellationToken cancellationToken = default) {
            var messages = new List<ChatMessage> {
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(userPrompt)
            };

            var exporterOptions = new JsonSchemaExporterOptions {
                TreatNullObliviousAsNonNullable = true,
                TransformSchemaNode = (context, schema) => {
                    if (schema is JsonObject obj && obj["type"]?.GetValue<string>() == "object") {
                        obj["additionalProperties"] = false;
                        if (obj["properties"] is JsonObject properties) obj["required"] = new JsonArray(properties.Select(x => JsonValue.Create(x.Key)).ToArray());
                    }
                    return schema;
                }
            };

            var schema = JsonSchemaExporter.GetJsonSchemaAsNode(JsonSerializerOptions.Default, typeof(T), exporterOptions);

            var options = new ChatCompletionOptions {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(jsonSchemaFormatName: typeof(T).Name, jsonSchema: BinaryData.FromString(schema.ToJsonString()), jsonSchemaIsStrict: true)
            };

            var startedAt = Stopwatch.GetTimestamp();
            
            var response = await _chatClient.CompleteChatAsync(messages,options,cancellationToken);
            
            var elapsed = Stopwatch.GetElapsedTime(startedAt);
            var usage = response.Value.Usage;
            _logger.LogInformation("AI request completed. Deployment={Deployment} ElapsedMilliseconds={ElapsedMilliseconds} InputTokens={InputTokens} OutputTokens={OutputTokens} TotalTokens={TotalTokens}.", _aiOptions.Deployment, elapsed.TotalMilliseconds, usage.InputTokenCount, usage.OutputTokenCount, usage.TotalTokenCount);

            var json = response.Value.Content[0].Text;

            var result = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true
                });

            if (result is null) {
                throw new InvalidOperationException($"AI response could not be deserialized to {typeof(T).Name}.");
            }

            return result;
        }
    }
}
