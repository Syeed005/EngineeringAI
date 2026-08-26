using EngineeringAI.Application.Models.Rag;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Interfaces.Search {
    public interface IEngineeringSearchIndexer {
        Task UploadAsync(IEnumerable<EngineeringDocumentChunk> chunks, CancellationToken cancellationToken = default);
    }
}
