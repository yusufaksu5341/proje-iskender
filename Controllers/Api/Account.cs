using ProjeIskender.Models.Account;
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
        /*
         * request değişkeninden gelen verileri veritabanından okuyup token oluşturması gerekiyor. Şimdilik
         * veritabanı işlemlerini userService değişkeni ile yap.
         *
         * Bu fonksiyon Authorization gerektirmiyor. Zaten token'ı olmayan birisi bu endpoint'i kullanacağı için 
         * token validation işlemi yapmayız.
         *
         * Bu yorumları fonksiyonu tekrar yazdıktan sonra sil.
         */
        var userIdClaim = User.FindFirst("UserID");

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var parsedUserId))
        {
            return Unauthorized("UserID claim bulunamadı!");
        }

        var token = new JwtToken()
        {
            UserID = parsedUserId,
            Expiration = DateTime.UtcNow.AddHours(1)
        };
        var jwt = JwtToken.Serialize(token);
        return Ok(jwt);
    }

    [HttpGet("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        throw new NotImplementedException();
    }
}
