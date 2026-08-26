using EngineeringAI.Application.Interfaces.Search;
using Microsoft.AspNetCore.Mvc;

namespace EngineeringAI.Api.Controllers {
    [ApiController]
    [Route("api/rag")]
    public class RagSearchController : ControllerBase {
        private readonly IEngineeringSearchService _searchService;

        public RagSearchController(IEngineeringSearchService searchService) {
            _searchService = searchService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string question, [FromQuery] int top = 5, CancellationToken cancellationToken = default) {
            var results = await _searchService.SearchAsync(question, top, cancellationToken);
            return Ok(results);
        }
    }
}
