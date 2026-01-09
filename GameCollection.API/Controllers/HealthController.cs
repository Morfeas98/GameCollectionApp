using Microsoft.AspNetCore.Mvc;

namespace GameCollection.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                status = "healthy",
                message = "Game Collection API is running",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
