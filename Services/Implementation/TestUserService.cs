using ProjeIskender.Services;
using ProjeIskender.Models;

namespace ProjeIskender.Services.Implementation;

[TestableClass]
class TestUserService : IUserService
{
    /*
     * TODO: Veritabanı bağlantısı eklendiği zaman bu işlemleri gerçek veritabanı ile eşleştir
     */
    private Dictionary<string, UserData> testData = new Dictionary<string, UserData>();

    public UserData? GetUserById(string userId)
    {
        if (userId == null)
        {
            throw new NullReferenceException("userId is null");
        }
        if (testData.TryGetValue(userId, out UserData user)) 
        {
            return user;
        }
        return null;
    }

    public bool ValidateUser(string userId, string userPassword)
    {
        var user = GetUserById(userId);
        if (user == null)
            return false;
        return user.UserPassword == userPassword;
    }

    public bool AddUser(UserData user)
    {
        throw new NotImplementedException();
    }

    private static TestUserService testService;

    [TestInit]
    public static void TestInit()
    {
        testService = new TestUserService();
        testService.testData.Add("test-0", new UserData() 
        {
            UserId = "test-0",
            UserPassword = "test1234",
            UserRole = "Guest"
        });
        testService.testData.Add("test-1", new UserData() 
        {
            UserId = "test-1",
            UserPassword = "asdf1234",
            UserRole = "Guest"
        });
        testService.testData.Add("penguen", new UserData()
        {
            UserId = "penguen",
            UserPassword = "penguen-lover-49",
            UserRole = "Admin"
        });
    }

    [TestCase]
    public static bool GetUserTestCorrect()
    {
        return testService.GetUserById("penguen")!.UserId == "penguen";
    }

    [TestCase]
    public static bool GetUserTestIncorrect()
    {
        return testService.GetUserById("marti") == null;
    }

    [TestCase]
    public static bool ValidateUserTestCorrect()
    {
        return testService.ValidateUser("penguen", "penguen-lover-49");
    }

    [TestCase]
    public static bool ValidateUserTestIncorrect()
    {
        return testService.ValidateUser("penguen", "pablo") == false;
    }
}
