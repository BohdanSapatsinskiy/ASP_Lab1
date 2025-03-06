using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webApiPr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyController : ControllerBase
    {
        [HttpGet]
        [Route("who")]
        public IActionResult GetWho()
        {
            return Ok(new { Name = "Богдан", Surname = "Сапацінський"});
        }


        [HttpGet]
        [Route("time")]
        public IActionResult GetTime()
        {
            return Ok(new { CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
        }
    }
}
