using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Options {
    public sealed class AiSearchOptions {
        public const string SectionName = "AiSearch";
        public string Endpoint { get; set; } = string.Empty;
        public string IndexName { get; set; } = string.Empty;
    }
}
}
