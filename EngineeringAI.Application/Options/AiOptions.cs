using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Options {
    public sealed class AiOptions {
        public const string SectionName = "AI";

        public string Endpoint { get; set; } = string.Empty;
        public string Deployment { get; set; } = string.Empty;
        public string EmbeddingDeployment { get; set; } = string.Empty;
    }
}
