using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Minerva_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult Get()
        {
            return Ok(new
            {
                status = "healthy"
            });
        }
    }
}
