namespace ProjeIskender.Models.Dto;

public class UserData
{
    required public ulong UserId { get; set; }
    required public string UserName { get; set; }
    required public string UserMail { get; set; }
    required public string UserPassword { get; set; }
    required public string UserRole { get; set; }
}
