using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            var userIdClaim = User.FindFirst("UserID");

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var parsedUserId))
            {
                return Unauthorized("UserID claim bulunamadı!");
            }

            var token = new JwtToken
            {
                UserID = parsedUserId,
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
        [Authorize]
        [HttpGet]
        public IActionResult Index(string jwt)
        {
            var isValid = JwtToken.Validate(jwt);
            return Ok(isValid);
        }
    }

}
