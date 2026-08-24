using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Interfaces.AI {
    public interface IEngineeringAiClient {
        Task<string> GenerateAsync(string systemPrompt, string userPrompt, CancellationToken cancellationToken = default);
    }
}
