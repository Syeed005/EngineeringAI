using Azure.Identity;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using EngineeringAI.Application.Interfaces.AI;
using EngineeringAI.Application.Interfaces.Search;
using EngineeringAI.Application.Models.Rag;
using EngineeringAI.Application.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Infrastructure.Search {
    public class AzureEngineeringSearchService : IEngineeringSearchService {
        private readonly SearchClient _searchClient;
        private readonly IEmbeddingClient _embeddingClient;

        public AzureEngineeringSearchService(IOptions<AiSearchOptions> options, IEmbeddingClient embeddingClient) {
            var searchOptions = options.Value;
            _embeddingClient = embeddingClient;
            _searchClient = new SearchClient(new Uri(searchOptions.Endpoint), searchOptions.IndexName, new DefaultAzureCredential());
        }

        public async Task<IReadOnlyList<EngineeringSearchResult>> SearchAsync(string question, int top = 5, CancellationToken cancellationToken = default) {
            var queryVector = await _embeddingClient.GenerateEmbeddingAsync(question, cancellationToken);

            var options = new SearchOptions {
                Size = top,
                Select = { "id", "documentId", "title", "content", "documentType", "equipmentNumber", "projectId", "supplier", "sourceFile", "chunkNumber" },
                VectorSearch = new VectorSearchOptions()
            };

            options.VectorSearch.Queries.Add(new VectorizedQuery(queryVector.ToArray()) { KNearestNeighborsCount = top, Fields = { "contentVector" } });

            var response = await _searchClient.SearchAsync<SearchDocument>(question, options, cancellationToken);

            var results = new List<EngineeringSearchResult>();

            await foreach (var result in response.Value.GetResultsAsync()) {
                var document = result.Document;

                results.Add(new EngineeringSearchResult {
                    Id = document["id"]?.ToString() ?? string.Empty,
                    DocumentId = document["documentId"]?.ToString() ?? string.Empty,
                    Title = document["title"]?.ToString() ?? string.Empty,
                    Content = document["content"]?.ToString() ?? string.Empty,
                    DocumentType = document["documentType"]?.ToString() ?? string.Empty,
                    EquipmentNumber = document["equipmentNumber"]?.ToString(),
                    ProjectId = document["projectId"] is int projectId ? projectId : null,
                    Supplier = document["supplier"]?.ToString(),
                    SourceFile = document["sourceFile"]?.ToString() ?? string.Empty,
                    ChunkNumber = document["chunkNumber"] is int chunkNumber ? chunkNumber : 0,
                    Score = result.Score ?? 0
                });
            }

            return results;
        }
    }
}
