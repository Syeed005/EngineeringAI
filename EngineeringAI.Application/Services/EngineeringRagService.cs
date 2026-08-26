using EngineeringAI.Application.DTOs.AI;
using EngineeringAI.Application.Interfaces.AI;
using EngineeringAI.Application.Interfaces.Search;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Services {
    public class EngineeringRagService {
        private readonly IEngineeringSearchService _searchService;
        private readonly IEngineeringAiClient _aiClient;

        public EngineeringRagService(IEngineeringSearchService searchService, IEngineeringAiClient aiClient) {
            _searchService = searchService;
            _aiClient = aiClient;
        }

        public async Task<EngineeringRagResponse> AskAsync(string question, CancellationToken cancellationToken = default) {
            var searchResults = await _searchService.SearchAsync(question, 5, cancellationToken);

            if (searchResults.Count == 0)
                return new EngineeringRagResponse { Answer = "No relevant engineering information was found." };

            var context = string.Join("\n\n", searchResults.Select((x, index) => $"SOURCE {index + 1}\nTitle: {x.Title}\nFile: {x.SourceFile}\nEquipment: {x.EquipmentNumber}\nContent:\n{x.Content}"));

            var systemPrompt =
                """
                You are an engineering assistant.
                Answer the user's question only from the supplied engineering context.
                Do not invent technical facts.
                If the context does not contain enough information, clearly state that the available documents do not provide enough information.
                """;

            var userPrompt = $"QUESTION:\n{question}\n\nENGINEERING CONTEXT:\n{context}";

            var aiResult = await _aiClient.GenerateStructuredAsync<EngineeringRagAiResponse>(systemPrompt, userPrompt, cancellationToken);

            return new EngineeringRagResponse {
                Answer = aiResult.Answer,
                Sources = searchResults.Select(x => x.SourceFile).Distinct().ToList()
            };
        }

    }
}
