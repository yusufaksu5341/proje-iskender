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

        var token = new JwtToken()
        {
            UserID = user.UserId,
            UserRole = (byte)user.UserRole,
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
            UserRole = UserRoles.MEMBER
        });

        if(!AddUser)
        {
            return BadRequest("Kullanıcı oluşturulurken bir hata oluştu!");
        }

        return Ok("Kullanıcı başarıyla oluşturuldu!");
    }

    [Authentication]
    [HttpGet("{userId?}")]
    public IActionResult GetUser(ulong? userId)
    {
        ulong resolvedUserId;

        if (userId == null)
        {
            resolvedUserId = (ulong)HttpContext.Items["UserID"]!;
        }
        else
        {
            resolvedUserId = userId.Value;
        }

        UserData? requestedUser = userService.GetUserById(resolvedUserId);
        if (requestedUser == null)
        {
            return NotFound("Kullanıcı bulunamadı!");
        }

        return Ok(new UserData()
        {
            UserId = requestedUser.UserId,
            UserName = requestedUser.UserName,
            UserMail = requestedUser.UserMail,
            UserRole = requestedUser.UserRole,
            UserPassword = null!,
            pictureUrl = requestedUser.pictureUrl
        });
    }

    [HttpPost("{userId}/verify-email/{mailCode}")]
    public IActionResult VerifyEmail(ulong UserId, string mailCode)
    {
        var user = userService.GetUserById(UserId);
        if (user == null)
        {
            return NotFound("Kullanıcı bulunamadı!");
        }
        
        var verificationResult = userService.VerifyEmail(user.UserMail, mailCode);
        
        if (!verificationResult)
        {
            return BadRequest("Email doğrulama başarısız!");
        }
        return Ok("Email doğrulama başarılı!");
    }

    [Authentication]
    [HttpGet("search")]
    public IActionResult SearchUsers([FromQuery] string userName)
    {
        var users = userService.SearchUsers(userName);
        return Ok(users);
    }

    [Authentication]
    [HttpPut("{userId}/picture")]
    public IActionResult UploadProfilePicture(ulong userId, [FromBody] string pictureUrl) 
    {
        if(userId != (ulong)HttpContext.Items["UserID"]!)
        {
            return Forbid("Kendi profil resminizi güncelleyebilirsiniz!");
        }

        var profileUpdated = userService.UpdateUserPicture(userId, pictureUrl);
        
        if (!profileUpdated)
        {
            return BadRequest("Profil resmi güncellenirken bir hata oluştu!");
        }
        return Ok("Profil resmi başarıyla güncellendi!");
    }

}

