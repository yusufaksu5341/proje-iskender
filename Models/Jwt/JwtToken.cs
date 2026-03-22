namespace ProjeIskender.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

[TestableClass]
public class JwtToken
{
    private static string key; // Değişken tipi değiştirilebilir

    public int UserID { get; set; }
    public DateTime Expiration { get; set; }

    public static void LoadKey(string key) // Key kontrolü ve atanması
    {
        if (string.IsNullOrEmpty(key) || key.Length < 32)
        {
            throw new ArgumentException("Key en az 256 bit (32 karakter) uzunluğunda olmalıdır!");
        }
        JwtToken.key = key;
    }

    public static string Serialize(JwtToken token) // Token nesnesini JWT stringine dönüştürme
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
            new Claim("UserID", token.UserID.ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtToken.key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(claims: claims, expires: token.Expiration, signingCredentials: credentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(jwtToken);
    }
    
    public static JwtToken Deserialize(string jwt)
    {
        throw new NotImplementedException();
    }

    public static bool Validate(string jwt)
    {
        throw new NotImplementedException();
    }

    [TestInit]
    public static void TestInit() 
    {
        JwtToken.LoadKey("a-string-secret-at-least-256-bits-long");
    }

    [TestCase]
    public static bool TestDeserialize()
    {
        return false;
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
        return false;
    }
}
