namespace ProjeIskender.Models;

[TestableClass]
public class JwtToken
{
    private static string key; // Değişken tipi değiştirilebilir

    public static void LoadKey(string key)
    {
        throw new NotImplementedException();
    }

    public static string Serialize(JwtToken token)
    {
        throw new NotImplementedException();
    }
    
    public static JwtToken Deserialize(string jwt)
    {
        throw new NotImplementedException();
    }

    public static bool Validate(string jwt)
    {
        throw new NotImplementedException();
    }

    [TestInit]
    public static void TestInit() 
    {
        JwtToken.LoadKey("a-string-secret-at-least-256-bits-long");
    }

    [TestCase]
    public static bool TestDeserialize()
    {
        return false;
    }
    
    [TestCase]
    public static bool TestSerialize()
    {
        return false;
    }

    [TestCase]
    public static bool TestValidate()
    {
        return false;
    }
}
