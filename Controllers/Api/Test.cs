using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ProjeIskender.Attributes;
using ProjeIskender.Models;

namespace ProjeIskender.Controllers.Api
{
    [Route("api/test")]
    [ApiController]
    public class Test : ControllerBase
    {
        [HttpGet]
        public IActionResult Index() 
        {
            return Ok("test");
        }

        [HttpPost("image")]
        [ContentAccept("image/png")]
        public IActionResult Image()
        {
            return Ok("Success!");
        }
    }
}
