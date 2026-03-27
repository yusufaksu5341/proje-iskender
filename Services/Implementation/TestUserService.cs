using ProjeIskender.Services;
using ProjeIskender.Models;

namespace ProjeIskender.Services.Implementation;

class TestUserService : IUserService
{
    /*
     * TODO: Veritabanı bağlantısı eklendiği zaman bu işlemleri gerçek veritabanı ile eşleştir
     */
    private Dictionary<string, UserData> testData = new Dictionary<string, UserData>();

    public UserData GetUserById(string userId)
    {
        throw new NotImplementedException();
    }

    public bool ValidateUser(string userId, string userPassword)
    {
        throw new NotImplementedException();
    }

    public bool AddUser(UserData user)
    {
        throw new NotImplementedException();
    }
}
