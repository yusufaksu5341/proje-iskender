using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjeIskender.Controllers.Api
{
    [Route("api/test")]
    [ApiController]
    public class Test : ControllerBase
    {
        [HttpGet]
        [HttpGet("{ping}")]
        public IActionResult Index(string ping = "pong") 
        {
            return Ok(ping);
        }
    }
}
