using ProjeIskender.Models.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjeIskender.Controllers.Api;

[ApiController]
[Route("jwt")]
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

        var token = new JwtToken
        {
            UserID = parsedUserId,
            Expiration = DateTime.UtcNow.AddHours(1)
        };
        var jwt = JwtToken.Serialize(token);
        return Ok(jwt);
    }
}
