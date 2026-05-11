using System.Buffers.Text;
using System.Security.Cryptography;
using ProjeIskender.Services;
using ProjeIskender.Models.Dto;
using System.Text;
using ProjeIskender.Models;
using BCrypt.Net;

namespace ProjeIskender.Services.Implementation;

[TestableClass]
public class TestUserService : IUserService
{
    /*
     * TODO: Veritabanı bağlantısı eklendiği zaman bu işlemleri gerçek veritabanı ile eşleştir
     */
    private static ulong _lastId;

    public static ulong lastId
    {
        get => _lastId;
        set => _lastId = value;
    }
    private static Dictionary<ulong, UserData>? _testData;

    public static Dictionary<ulong, UserData> testData
    {
        get
        {
            if (_testData == null)
                _testData = new();
            return _testData;
        }
    }
    private byte[] emailKey;

    public TestUserService(byte[] emailKey)
    {
        this.emailKey = emailKey;
    }

    public UserData? GetUserById(ulong userId)
    {
        if (testData.TryGetValue(userId, out var user)) 
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

        return testData.FirstOrDefault(x => x.Value.UserName == userName).Value;
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

        return testData.FirstOrDefault(x => x.Value.UserMail == userEmail).Value;
    }

    public bool ValidateUser(ulong userId, string userPassword)
    {
        var user = GetUserById(userId);
        if (user == null)
            return false;
        return BCrypt.Net.BCrypt.Verify(userPassword, user.UserPassword);
    }
    
    public bool ValidateUser(string userName, string userPassword)
    {
        var user = GetUserByName(userName);
        if (user == null)
            return false;
        return BCrypt.Net.BCrypt.Verify(userPassword, user.UserPassword);
    }
    
    public bool ValidateUserByEmail(string email, string userPassword)
    {
        var user = GetUserByEmail(email);
        if (user == null)
            return false;
        return BCrypt.Net.BCrypt.Verify(userPassword, user.UserPassword);
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
        try
        {
            _ = testData.First(x => x.Value.UserMail == user.UserMail || x.Value.UserName == user.UserName);

            return false;
        }
        catch (Exception)
        { }
        
        ulong id = lastId++;
        user.UserId = id;
        user.UserPassword = BCrypt.Net.BCrypt.HashPassword(user.UserPassword);
        testData.Add(id, user);

        return true;
    }

    public bool UpdateUserPicture(ulong userId, string pictureUrl)
    {
        if (testData.TryGetValue(userId, out var user))
        {
            user.PictureUrl = pictureUrl;
            return true;
        }
        return false;
    }

    private static TestUserService testService = null!;

    [TestInit]
    public static void TestInit()
    {
        testService = new TestUserService(Encoding.UTF8.GetBytes("test-key"));
        testData.Add(0, new UserData() 
        {
            UserId = 0,
            UserName = "test0",
            UserMail = "test1@email.com",
            UserPassword = "test1234",
            UserRole = UserRoles.MEMBER
        });
        testData.Add(1, new UserData() 
        {
            UserId = 1,
            UserName = "test1",
            UserMail = "test2@email.com",
            UserPassword = "asdf1234",
            UserRole = UserRoles.MEMBER
        });
        testData.Add(2, new UserData()
        {
            UserId = 2,
            UserName = "penguen",
            UserMail = "los-penguenos@email.com",
            UserPassword = "penguen-lover-49",
            UserRole = UserRoles.ADMIN
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