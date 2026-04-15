using ProjeIskender.Models.Account;
using ProjeIskender.Models.Dto;
using ProjeIskender.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjeIskender.Controllers.Api;

[ApiController]
[Route("api/account")]
public class Account : ControllerBase
{
    ILogger<Account> logger;
    IUserService userService;
    
    public Account(ILogger<Account> logger, IUserService userService)
    {
        this.logger = logger;
        this.userService = userService;
    }

    [HttpGet("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        UserData? user = null;

        if (request.NameType == 0)
        {
            if(!userService.ValidateUserByEmail(request.Name, request.Password))
            {
                return BadRequest("Geçersiz email veya şifre!");
            }
            user = userService.GetUserByEmail(request.Name);
        }
        else if (request.NameType == 1)
        {
            if(!userService.ValidateUser(request.Name, request.Password))
            {
                return BadRequest("Geçersiz kullanıcı adı veya şifre!");
            }
            user = userService.GetUserByName(request.Name);
        }
        else
        {
            return BadRequest("Geçersiz NameType değeri!");
        }
        
        var role = string.Equals(user!.UserRole, "Admin", StringComparison.OrdinalIgnoreCase) ? 1 : 0;

        var token = new JwtToken()
        {
            UserID = (int)user.UserId!,
            UserRole = role,
            Expiration = DateTime.UtcNow.AddHours(1)
        };

        var jwt = JwtToken.Serialize(token);
        return Ok(jwt);
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (userService.GetUserByEmail(request.UserMail) != null)
        {
            return BadRequest("Bu email zaten kayıtlı!");
        }

        if (userService.GetUserByName(request.UserName) != null)
        {
            return BadRequest("Bu kullanıcı adı zaten kayıtlı!");
        }

        var AddUser = userService.AddUser(new UserData()
        {
            UserMail = request.UserMail,
            UserName = request.UserName,
            UserPassword = request.UserPassword,
            UserRole = "Guest"
        });

        if(!AddUser)
        {
            return BadRequest("Kullanıcı oluşturulurken bir hata oluştu!");
        }

        return Ok("Kullanıcı başarıyla oluşturuldu!");
    }
}
