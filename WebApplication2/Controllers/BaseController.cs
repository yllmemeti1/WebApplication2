using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class BaseController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult Protected()
        {
            return Ok("This is a protected route");
        }
    }
}
