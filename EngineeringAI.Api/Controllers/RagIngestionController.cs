using EngineeringAI.Application.Models.Rag;
using EngineeringAI.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EngineeringAI.Api.Controllers {
    [ApiController]
    [Route("api/rag")]
    public class RagIngestionController : ControllerBase {
        private readonly EngineeringDocumentIngestionService _ingestionService;

        public RagIngestionController(EngineeringDocumentIngestionService ingestionService) {
            _ingestionService = ingestionService;
        }

        [HttpPost("ingest-test-document")]
        public async Task<IActionResult> IngestTestDocument(CancellationToken cancellationToken) {
            var document = new EngineeringDocument {
                DocumentId = "DOC-PUMP-001",
                Title = "EQ-RAG-001 Centrifugal Cooling Water Pump Specification",
                DocumentType = "EquipmentSpecification",
                EquipmentNumber = "EQ-RAG-001",
                ProjectId = 1,
                Supplier = "Apex Industrial",
                SourceFile = "EQ-RAG-001_Pump_Specification.txt",
                Content =
                    """
                    Equipment EQ-RAG-001 is a centrifugal cooling water pump supplied by Apex Industrial.

                    The pump is designed for continuous cooling water circulation service. The rated flow is 450 cubic meters per hour and the rated discharge pressure is 6.5 bar. The maximum allowable operating temperature is 85 degrees Celsius.

                    The pump shall not operate continuously below 30 percent of its best efficiency point flow. Extended operation below minimum flow can cause excessive vibration, internal recirculation, seal damage, and increased bearing temperature.

                    Mechanical seal inspection shall be performed every six months. The inspection shall include seal leakage, seal face condition, flush line condition, and abnormal temperature. Mechanical seals showing excessive leakage or visible damage shall be replaced.

                    Bearing lubrication shall be inspected every three months. Bearing temperature shall be monitored during normal operation and investigated if it exceeds the supplier recommended operating range.

                    The suction strainer shall be inspected during scheduled maintenance outages. Excessive strainer blockage can reduce suction pressure and increase the risk of cavitation.

                    During commissioning, verify pump rotation, coupling alignment, lubrication condition, suction valve position, discharge valve condition, and adequate system fill before startup.

                    Operators shall investigate abnormal vibration, excessive noise, loss of discharge pressure, seal leakage, or unexpected motor current increase before continuing operation.

                    Apex Industrial recommends maintaining inspection records for all mechanical seal, bearing, alignment, and vibration checks throughout the equipment lifecycle.
                    """
            };

            await _ingestionService.IngestAsync(document, cancellationToken);

            return Ok(new {
                Message = "Synthetic engineering document ingested successfully.",
                document.DocumentId,
                document.Title,
                document.EquipmentNumber
            });
        }
    }
}
