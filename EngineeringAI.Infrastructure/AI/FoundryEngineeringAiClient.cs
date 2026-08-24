using Azure.AI.OpenAI;
using Azure.Identity;
using EngineeringAI.Application.Interfaces.AI;
using EngineeringAI.Application.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Infrastructure.AI {
    public class FoundryEngineeringAiClient : IEngineeringAiClient {
        private readonly ChatClient _chatClient;

        public FoundryEngineeringAiClient(IOptions<AiOptions> options) {
            var aiOptions = options.Value;

            var azureClient = new AzureOpenAIClient(
            new Uri(aiOptions.Endpoint),
            new DefaultAzureCredential());

            _chatClient = azureClient.GetChatClient(aiOptions.Deployment);
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
