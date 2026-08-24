using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Options {
    public sealed class ServiceBusOptions {
        public const string SectionName = "ServiceBus";

        public string FullyQualifiedNamespace { get; set; } = string.Empty;
        public string EquipmentEventsQueue { get; set; } = string.Empty;
    }
}
