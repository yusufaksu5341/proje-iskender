using ProjeIskender.Models.Dto;

namespace ProjeIskender.Services;

public interface IUserService
{
    public UserData? GetUserById(ulong userId);
    public UserData? GetUserByName(string userName);
    public IEnumerable<UserData> SearchUsers(string userName);
    public UserData? GetUserByEmail(string userEmail);

    public bool ValidateUser(ulong userId, string userPassword);
    public bool ValidateUser(string userName, string userPassword);
    public bool ValidateUserByEmail(string email, string userPassword);

    public string GenerateEmailVerification(string email);

    public bool VerifyEmail(string email, string mailCode);
}
