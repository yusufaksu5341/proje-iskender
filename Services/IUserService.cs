using ProjeIskender.Models;

namespace ProjeIskender.Services;

public interface IUserService
{
    public UserData? GetUserById(string userId);
    public UserData? GetUserByEmail(string email);
    public bool ValidateUser(string userId, string userPassword);

    public bool AddUser(UserData user);
}
