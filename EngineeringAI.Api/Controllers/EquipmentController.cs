using EngineeringAI.Api.Filters;
using EngineeringAI.Application.DTOs.Equipment;
using EngineeringAI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EngineeringAI.Api.Controllers {
    //Use the service interface, not the repository or DbContext directly
    [ApiController]
    [Route("api/equipment")]
    public class EquipmentController : ControllerBase {
        private readonly IEquipmentService _equipmentService;
        public EquipmentController(IEquipmentService equipmentService) {
            _equipmentService = equipmentService;
        }
        [HttpGet]
        [ServiceFilter(typeof(ValidationFilter<EquipmentQueryParameters>))]
        public async Task<IActionResult> Get([FromQuery] EquipmentQueryParameters queryParameters, CancellationToken cancellationToken) {
            var result = await _equipmentService.GetAsync(queryParameters, cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken) {
            var equipment = await _equipmentService.GetByIdAsync(id, cancellationToken);

            if (equipment == null)
                return NotFound();

            return Ok(equipment);
        }

        [HttpGet("project/{projectId:int}")]
        public async Task<IActionResult> GetByProjectId(int projectId, CancellationToken cancellationToken) {
            var equipment = await _equipmentService.GetByProjectIdAsync(projectId, cancellationToken);

            return Ok(equipment);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilter<CreateEquipmentRequest>))]
        public async Task<IActionResult> Create(CreateEquipmentRequest request, CancellationToken cancellationToken) {
            var equipment = await _equipmentService.CreateAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = equipment.Id }, equipment);
        }

        [HttpPut("{id:int}")]
        [ServiceFilter(typeof(ValidationFilter<UpdateEquipmentRequest>))]
        public async Task<IActionResult> Update(int id, UpdateEquipmentRequest request, CancellationToken cancellationToken) {
            var equipment = await _equipmentService.UpdateAsync(id, request, cancellationToken);

            return Ok(equipment);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken) {
            await _equipmentService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
