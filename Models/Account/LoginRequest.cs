namespace ProjeIskender.Models.Account;

public class LoginRequest
{
    required public string Name { get; set; }
    required public string Password { get; set; }
    required public byte NameType { get; set; } // Mail: 0, UserName: 1
}
