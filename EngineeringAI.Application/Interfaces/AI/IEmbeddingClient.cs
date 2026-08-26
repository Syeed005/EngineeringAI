using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Interfaces.AI {
    public interface IEmbeddingClient {
        Task<IReadOnlyList<float>> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default);
    }
}
