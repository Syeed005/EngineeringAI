using EngineeringAI.Application.Interfaces.Search;
using EngineeringAI.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EngineeringAI.Api.Controllers {
    [ApiController]
    [Route("api/rag")]
    public class RagSearchController : ControllerBase {
        private readonly IEngineeringSearchService _searchService;
        private readonly EngineeringRagService _ragService;

        public RagSearchController(IEngineeringSearchService searchService, EngineeringRagService ragService) {
            _searchService = searchService;
            _ragService = ragService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string question, [FromQuery] int top = 5, CancellationToken cancellationToken = default) {
            var results = await _searchService.SearchAsync(question, top, cancellationToken);
            return Ok(results);
        }

        [HttpGet("ask")]
        public async Task<IActionResult> Ask([FromQuery] string question, CancellationToken cancellationToken = default) {
            var result = await _ragService.AskAsync(question, cancellationToken);
            return Ok(result);
        }
    }
}
