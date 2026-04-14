using System.Buffers.Text;
using System.Security.Cryptography;
using ProjeIskender.Services;
using ProjeIskender.Models.Dto;
using System.Text;

namespace ProjeIskender.Services.Implementation;

[TestableClass]
class TestUserService : IUserService
{
    /*
     * TODO: Veritabanı bağlantısı eklendiği zaman bu işlemleri gerçek veritabanı ile eşleştir
     */
    private Dictionary<ulong, UserData> testData = new Dictionary<ulong, UserData>();
    private static byte[] emailKey;

    public UserData? GetUserById(ulong userId)
    {
        if (testData.TryGetValue(userId, out UserData user)) 
        {
            return user;
        }
        return null;
    }

    public UserData? GetUserByName(string userName)
    {
        if (userName == null)
        {
            throw new NullReferenceException("userName is null");
        }

        UserData? user;
        if ((user = testData.First(x => x.Value.UserName == userName).Value) != null) 
        {
            return user;
        }
        return null;
    }

    public IEnumerable<UserData> SearchUsers(string userName)
    {
        if (userName == null)
        {
            throw new NullReferenceException("userName is null");
        }

        return testData.Where(x => x.Value.UserName.Contains(userName)).Select(x => x.Value);
    }
    
    public UserData? GetUserByEmail(string userEmail)
    {
        if (userEmail == null)
        {
            throw new NullReferenceException("userEmail is null");
        }

        UserData? user;
        if ((user = testData.First(x => x.Value.UserMail == userEmail).Value) != null) 
        {
            return user;
        }
        return null;
    }

    public bool ValidateUser(ulong userId, string userPassword)
    {
        var user = GetUserById(userId);
        if (user == null)
            return false;
        return user.UserPassword == userPassword;
    }
    
    public bool ValidateUser(string userName, string userPassword)
    {
        var user = GetUserByName(userName);
        if (user == null)
            return false;
        return user.UserPassword == userPassword;
    }
    
    public bool ValidateUserByEmail(string email, string userPassword)
    {
        var user = GetUserByEmail(email);
        if (user == null)
            return false;
        return user.UserPassword == userPassword;
    }

    public string GenerateEmailVerification(string email)
    {
        return Base64Url.EncodeToString(HMACSHA256.HashData(emailKey, Encoding.UTF8.GetBytes(email)));
    }

    public bool VerifyEmail(string email, string mailCode)
    {
        if (email == null || mailCode == null)
        {
            return false;
        }

        return Base64Url.EncodeToString(HMACSHA256.HashData(emailKey, Encoding.ASCII.GetBytes(email))) == mailCode;
    }

    public bool AddUser(UserData user)
    {
        throw new NotImplementedException();
    }

    private static TestUserService testService;

    [TestInit]
    public static void TestInit()
    {
        emailKey = Encoding.UTF8.GetBytes("test-key");
        testService = new TestUserService();
        testService.testData.Add(0, new UserData() 
        {
            UserId = 0,
            UserName = "test0",
            UserMail = "test1@email.com",
            UserPassword = "test1234",
            UserRole = "Guest"
        });
        testService.testData.Add(1, new UserData() 
        {
            UserId = 1,
            UserName = "test1",
            UserMail = "test2@email.com",
            UserPassword = "asdf1234",
            UserRole = "Guest"
        });
        testService.testData.Add(2, new UserData()
        {
            UserId = 2,
            UserName = "penguen",
            UserMail = "los-penguenos@email.com",
            UserPassword = "penguen-lover-49",
            UserRole = "Admin"
        });
    }

    [TestCase]
    public static bool GetUserTestCorrect()
    {
        return testService.GetUserById(2)!.UserName == "penguen";
    }

    [TestCase]
    public static bool GetUserTestIncorrect()
    {
        return testService.GetUserById(8) == null;
    }

    [TestCase]
    public static bool ValidateUserTestCorrect()
    {
        return testService.ValidateUser(2, "penguen-lover-49");
    }

    [TestCase]
    public static bool ValidateUserTestIncorrect()
    {
        return testService.ValidateUser(2, "pablo") == false;
    }
}
