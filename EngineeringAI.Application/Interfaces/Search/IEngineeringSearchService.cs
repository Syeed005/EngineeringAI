using EngineeringAI.Application.Models.Rag;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Interfaces.Search {
    public interface IEngineeringSearchService {
        Task<IReadOnlyList<EngineeringSearchResult>> SearchAsync(string question, int top = 5, CancellationToken cancellationToken = default);
    }
}
