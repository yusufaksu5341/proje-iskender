using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ProjeIskender.Models.Account;

[TestableClass]
public class JwtToken
{
    private static string key; // Değişken tipi değiştirilebilir

    public int UserID { get; set; }
    public int UserRole { get; set; }
    public DateTime Expiration { get; set; }

    public static void LoadKey(string key)
    {
        if (string.IsNullOrEmpty(key) || key.Length < 32)
        {
            throw new ArgumentException("Key en az 256 bit (32 karakter) uzunluğunda olmalıdır!");
        }
        JwtToken.key = key;
    }

    public static string Serialize(JwtToken token) 
    {
        if (token == null)
        {
            throw new ArgumentNullException(nameof(token));
        }

        if (string.IsNullOrEmpty(JwtToken.key))
        {
            throw new InvalidOperationException("Key yüklenmemiş! Lütfen önce LoadKey metodunu kullanarak bir key yükleyin.");
        }

        var claims = new List<Claim>
        {
            new Claim("UserID", token.UserID.ToString()),
            new Claim("UserRole", token.UserRole.ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtToken.key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(claims: claims, expires: token.Expiration, signingCredentials: credentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(jwtToken);
    }
    
    public static JwtToken Deserialize(string jwt)
    {
        if(string.IsNullOrEmpty(jwt))
        {
            throw new ArgumentNullException("JWT boş olamaz!");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var jsonWebToken = tokenHandler.ReadJwtToken(jwt);
        var userIdClaim = jsonWebToken.Claims.FirstOrDefault(c => c.Type == "UserID");
        var userRoleClaim = jsonWebToken.Claims.FirstOrDefault(c => c.Type == "UserRole");

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var parsedUserId))
        {
            throw new SecurityTokenException("JWT UserID claim bulunamadı veya geçersiz!");
        }
        if (userRoleClaim == null || !int.TryParse(userIdClaim.Value, out var parsedUserRole))
        {
            throw new SecurityTokenException("JWT UserRole claim bulunamadı veya geçersiz!");
        }

        return new JwtToken
        {
            UserID = parsedUserId,
            UserRole = parsedUserRole,
            Expiration = jsonWebToken.ValidTo
        };
    }

    public static bool Validate(string jwt) 
    {
        if(string.IsNullOrEmpty(jwt) || string.IsNullOrEmpty(JwtToken.key))
        {
            return false;
        }
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtToken.key)),
            ValidateIssuerSigningKey = true
        };

        try
        {
            tokenHandler.ValidateToken(jwt, validationParameters, out SecurityToken validatedToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    [TestInit]
    public static void TestInit() 
    {
        JwtToken.LoadKey("a-string-secret-at-least-256-bits-long");
    }

    [TestCase]
    public static bool TestDeserialize()
    {
        var token = new JwtToken
        {
            UserID = 4,
            Expiration = DateTime.UtcNow.AddHours(1)
        };

        var jwt = JwtToken.Serialize(token);
        var deserializedToken = JwtToken.Deserialize(jwt);

        return deserializedToken.UserID == token.UserID;
    }
    
    [TestCase]
    public static bool TestSerialize()
    {
        var token = new JwtToken
        {
            UserID = 4,
            Expiration = DateTime.UtcNow.AddHours(1)
        };

        var jwt = JwtToken.Serialize(token);
        
        return !string.IsNullOrEmpty(jwt);
    }

    [TestCase]
    public static bool TestValidate()
    {
        var token = new JwtToken
        {
            UserID = 4,
            Expiration = DateTime.UtcNow.AddHours(1)
        };

        var jwt = JwtToken.Serialize(token);
        
        return JwtToken.Validate(jwt);
    }
}
