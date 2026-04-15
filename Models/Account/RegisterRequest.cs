namespace ProjeIskender.Models.Account;

public class RegisterRequest
{
    required public string UserMail { get; set; }
    required public string UserName { get; set; }
    required public string UserPassword { get; set; }
    required public string ConfirmPassword { get; set; }
}