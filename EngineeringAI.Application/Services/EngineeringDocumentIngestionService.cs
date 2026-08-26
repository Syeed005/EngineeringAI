using EngineeringAI.Application.Interfaces.AI;
using EngineeringAI.Application.Interfaces.Search;
using EngineeringAI.Application.Models.Rag;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Services {
    public class EngineeringDocumentIngestionService {
        private readonly IEmbeddingClient _embeddingClient;
        private readonly IEngineeringSearchIndexer _searchIndexer;

        public EngineeringDocumentIngestionService(IEmbeddingClient embeddingClient, IEngineeringSearchIndexer searchIndexer) {
            _embeddingClient = embeddingClient;
            _searchIndexer = searchIndexer;
        }

        public async Task IngestAsync(EngineeringDocument document, CancellationToken cancellationToken = default) {
            var textChunks = ChunkText(document.Content, 1000);
            var chunks = new List<EngineeringDocumentChunk>();

            for (var i = 0; i < textChunks.Count; i++) {
                var vector = await _embeddingClient.GenerateEmbeddingAsync(textChunks[i], cancellationToken);

                chunks.Add(new EngineeringDocumentChunk {
                    Id = $"{document.DocumentId}-{i}",
                    DocumentId = document.DocumentId,
                    Title = document.Title,
                    Content = textChunks[i],
                    DocumentType = document.DocumentType,
                    EquipmentNumber = document.EquipmentNumber,
                    ProjectId = document.ProjectId,
                    Supplier = document.Supplier,
                    SourceFile = document.SourceFile,
                    ChunkNumber = i,
                    ContentVector = vector
                });
            }

            await _searchIndexer.UploadAsync(chunks, cancellationToken);
        }
        private static List<string> ChunkText(string text, int chunkSize) {
            if (string.IsNullOrWhiteSpace(text))
                return [];

            var chunks = new List<string>();

            for (var start = 0; start < text.Length; start += chunkSize)
                chunks.Add(text.Substring(start, Math.Min(chunkSize, text.Length - start)));

            return chunks;
        }
    }
}
