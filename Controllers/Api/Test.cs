using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }

    [Route("api/createjwt")]
    [ApiController]
    public class CreateJwt : ControllerBase
    {
        [HttpGet]
        public IActionResult Index(int userID)
        {
            var token = new JwtToken
            {
                UserID = userID,
                Expiration = DateTime.UtcNow.AddHours(1)
            };
            var jwt = JwtToken.Serialize(token);
            return Ok(jwt);
        }
    }

    [Route("api/validatejwt")]
    [ApiController]
    public class ValidateJwt : ControllerBase
    {
        [HttpGet]
        public IActionResult Index(string jwt)
        {
            var isValid = JwtToken.Validate(jwt);
            return Ok(isValid);
        }
    }

}
