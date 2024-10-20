using Microsoft.AspNetCore.Mvc;

namespace Takerman.Notifications.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController(ILogger<HomeController> _logger) : ControllerBase
    {
        [HttpGet(Name = "Get")]
        public bool Get()
        {
            return true;
        }
    }
}
