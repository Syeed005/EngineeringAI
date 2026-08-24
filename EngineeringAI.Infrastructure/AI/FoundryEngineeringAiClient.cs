using Azure.AI.OpenAI;
using Azure.Identity;
using EngineeringAI.Application.Interfaces.AI;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Infrastructure.AI {
    public class FoundryEngineeringAiClient : IEngineeringAiClient {
        private readonly ChatClient _chatClient;

        public FoundryEngineeringAiClient(IConfiguration configuration) {
            var endpoint = configuration["AI:Endpoint"];
            var deployment = configuration["AI:Deployment"];

            if (string.IsNullOrWhiteSpace(endpoint))
                throw new InvalidOperationException("AI endpoint is not configured.");

            if (string.IsNullOrWhiteSpace(deployment))
                throw new InvalidOperationException("AI deployment is not configured.");

            var azureClient = new AzureOpenAIClient(
                new Uri(endpoint),
                new DefaultAzureCredential());

            _chatClient = azureClient.GetChatClient(deployment);
        }
        public async Task<string> GenerateAsync(string systemPrompt, string userPrompt, CancellationToken cancellationToken = default) {
            var messages = new List<ChatMessage>{
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(userPrompt)
            };

            var response = await _chatClient.CompleteChatAsync(messages, cancellationToken: cancellationToken);
            return response.Value.Content[0].Text;

        }
    }
}
