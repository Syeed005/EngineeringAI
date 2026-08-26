using Azure.Identity;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using EngineeringAI.Application.Interfaces.Search;
using EngineeringAI.Application.Models.Rag;
using EngineeringAI.Application.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Infrastructure.Search {
    public class AzureEngineeringSearchIndexer : IEngineeringSearchIndexer {
        private readonly SearchClient _searchClient;

        public AzureEngineeringSearchIndexer(IOptions<AiSearchOptions> options) {
            var searchOptions = options.Value;
            _searchClient = new SearchClient(new Uri(searchOptions.Endpoint), searchOptions.IndexName, new DefaultAzureCredential());
        }
        public async Task UploadAsync(IEnumerable<EngineeringDocumentChunk> chunks, CancellationToken cancellationToken = default) {
            var documents = chunks.Select(chunk => new SearchDocument {
                ["id"] = chunk.Id,
                ["documentId"] = chunk.DocumentId,
                ["title"] = chunk.Title,
                ["content"] = chunk.Content,
                ["documentType"] = chunk.DocumentType,
                ["equipmentNumber"] = chunk.EquipmentNumber,
                ["projectId"] = chunk.ProjectId,
                ["supplier"] = chunk.Supplier,
                ["sourceFile"] = chunk.SourceFile,
                ["chunkNumber"] = chunk.ChunkNumber,
                ["contentVector"] = chunk.ContentVector
            }).ToList();

            await _searchClient.UploadDocumentsAsync(documents, cancellationToken: cancellationToken);
        }
    }
}
