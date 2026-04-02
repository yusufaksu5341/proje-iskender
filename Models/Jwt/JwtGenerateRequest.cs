namespace ProjeIskender.Models.Jwt;

public class JwtGenerateRequest
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public required string Password { get; set; }
    
}
