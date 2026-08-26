using Azure.AI.OpenAI;
using Azure.Identity;
using EngineeringAI.Application.Interfaces.AI;
using EngineeringAI.Application.Options;
using Microsoft.Extensions.Options;
using OpenAI.Embeddings;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Infrastructure.AI {
    public class FoundryEmbeddingClient : IEmbeddingClient {
        private readonly EmbeddingClient _embeddingClient;
        

        public FoundryEmbeddingClient(IOptions<AiOptions> options) {
            var aiOptions = options.Value;
            var azureClient = new AzureOpenAIClient(new Uri(aiOptions.Endpoint), new DefaultAzureCredential());
            _embeddingClient = azureClient.GetEmbeddingClient(aiOptions.EmbeddingDeployment);
        }
        public async Task<IReadOnlyList<float>> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default) {
            var response = await _embeddingClient.GenerateEmbeddingAsync(text, cancellationToken: cancellationToken);
            return response.Value.ToFloats().ToArray();
        }
    }
}
