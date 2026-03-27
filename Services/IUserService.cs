using ProjeIskender.Models;

namespace ProjeIskender.Services;

public interface IUserService
{
    public UserData? GetUserById(string userId);
    public bool ValidateUser(string userId, string userPassword);

    public bool AddUser(UserData user);
}
