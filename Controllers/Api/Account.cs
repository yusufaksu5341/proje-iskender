using ProjeIskender.Models.Account;
using ProjeIskender.Models.Dto;
using ProjeIskender.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjeIskender.Models;
using Microsoft.AspNetCore.Authorization;
using ProjeIskender.Attributes;

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
        UserData user;

        if (request.NameType == 0)
        {
            if(!userService.ValidateUserByEmail(request.Name, request.Password))
            {
                return BadRequest("Geçersiz email veya şifre!");
            }
            user = userService.GetUserByEmail(request.Name)!;
        }
        else if (request.NameType == 1)
        {
            if(!userService.ValidateUser(request.Name, request.Password))
            {
                return BadRequest("Geçersiz kullanıcı adı veya şifre!");
            }
            user = userService.GetUserByName(request.Name)!;
        }
        else
        {
            return BadRequest("Geçersiz NameType değeri!");
        }
        
        try
        {
            var token = new JwtToken()
            {
                UserID = user.UserId,
                UserRole = (byte)user.UserRole,
                Expiration = DateTime.UtcNow.AddHours(1)
            };

            var jwt = JwtToken.Serialize(token);
            return Ok(jwt);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Token oluşturulurken bir hata oluştu!");
        }
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
            UserRole = UserRoles.MEMBER
        });

        if(!AddUser)
        {
            return BadRequest("Kullanıcı oluşturulurken bir hata oluştu!");
        }

        return Ok("Kullanıcı başarıyla oluşturuldu!");
    }

    [Authentication]
    [HttpGet]
    public IActionResult GetUser()
    {
        return GetUser(((JwtToken)HttpContext.Items["Jwt-Token"]!).UserID);
    }

    [Authentication]
    [HttpGet("{userId}")]
    public IActionResult GetUser(ulong userId)
    {
        UserData? requestedUser = userService.GetUserById(userId);
        if (requestedUser == null)
        {
            return NotFound("Kullanıcı bulunamadı!");
        }

        try
        {
            return Ok(new UserResult()
            {
                UserId = requestedUser.UserId,
                UserName = requestedUser.UserName
            });
        }
        catch (Exception e)
        {
            return NotFound();
        }
    }

    [HttpPost("{userId}/verify-email")]
    public IActionResult VerifyEmail(ulong UserId)
    {
        var user = userService.GetUserById(UserId);
        if (user == null)
        {
            return NotFound("Kullanıcı bulunamadı!");
        }

        try
        {
            var mailCode = userService.GenerateEmailVerification(user.UserMail);
            return Ok(mailCode);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Doğrulama kodu oluşturulurken bir hata oluştu!");
        }
    }

    [Authentication]
    [HttpGet("search")]
    public IActionResult SearchUsers([FromQuery] string userName)
    {
        try
        {
            var users = userService.SearchUsers(userName);
            return Ok(users.Select(u => new UserResult()
            {
                UserId = u.UserId,
                UserName = u.UserName,
            }));
        }
        catch (Exception e)
        {
            return NotFound();
        }
    }

}
