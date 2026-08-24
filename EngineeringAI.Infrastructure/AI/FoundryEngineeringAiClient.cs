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

namespace EngineeringAI.Infrastructure.AI {
    public class FoundryEngineeringAiClient : IEngineeringAiClient {
        private readonly ChatClient _chatClient;
        private readonly ILogger<FoundryEngineeringAiClient> _logger;

        public FoundryEngineeringAiClient(IOptions<AiOptions> options, ILogger<FoundryEngineeringAiClient> logger) {
            var aiOptions = options.Value;

            var azureClient = new AzureOpenAIClient(
            new Uri(aiOptions.Endpoint),
            new DefaultAzureCredential());

            _chatClient = azureClient.GetChatClient(aiOptions.Deployment);
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
    }
}
