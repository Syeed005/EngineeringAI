using Microsoft.AspNetCore.Mvc;

namespace EngineeringAI.Api.Controllers {
    [ApiController]
    [Route("api/system")]
    public class SystemController : ControllerBase {
        [HttpGet("version")]
        public IActionResult GetVersion() {
            return Ok(new {
                application = "EngineeringAI",
                version = "1.0.1",
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
            });
        }
    }
}
