using ProjeIskender.Models.Jwt;
using ProjeIskender.Models;
using ProjeIskender.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjeIskender.Controllers.Api;

[ApiController]
[Route("api/jwt")]
public class Jwt : ControllerBase
{
    ILogger<Jwt> logger;
    IUserService userService;
    
    public Jwt(ILogger<Jwt> logger, IUserService userService)
    {
        this.logger = logger;
        this.userService = userService;
    }

    [HttpGet("generate-token")]
    public IActionResult Generate([FromBody] JwtGenerateRequest request) 
    {   
        String UserId = request.UserId;
        if(UserId != null)
        {
            if(!userService.ValidateUser(UserId, request.Password))
            {
                return BadRequest("Kullanıcı adı veya şifre yanlış!");
            }

        }
        else if(request.Email != null)
        {   
            UserData? user;
            user = userService.GetUserByEmail(request.Email);

            if(user == null || !userService.ValidateUser(user!.Id, request.Password))
            {
                return BadRequest("Kullanıcı adı veya şifre yanlış!");
            }
            UserId = user.Id;
            
        }
        else
        {
            return BadRequest("Lütfen bilgileri eksiksiz giriniz!");
        }
        
        var token = new JwtToken()
        {
            UserID = UserId,
            Expiration = DateTime.UtcNow.AddHours(1)
        };

        var jwt = JwtToken.Serialize(token);
        return Ok(jwt);
    }
}
