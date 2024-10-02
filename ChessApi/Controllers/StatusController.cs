using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChessApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Content("Hello app");
        }
    }
}
