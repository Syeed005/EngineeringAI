using EngineeringAI.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EngineeringAI.Api.Controllers {
    [ApiController]
    [Route("api/equipment")]
    public class EquipmentAiController : ControllerBase {
        private readonly EquipmentAiService _equipmentAiService;

        public EquipmentAiController(EquipmentAiService equipmentAiService) {
            _equipmentAiService = equipmentAiService;
        }
        [HttpPost("{id:int}/ai-summary")]
        public async Task<IActionResult> GenerateSummary(int id, CancellationToken cancellationToken) {
            var result = await _equipmentAiService.GenerateSummaryAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}
