using Microsoft.EntityFrameworkCore;
using ProjeIskender.Context;
using ProjeIskender.Models;
using ProjeIskender.Models.Dto;
using BCrypt.Net;

namespace ProjeIskender.Services.Implementation;

public class UserService : IUserService
{
    private IskenderContext _context;
    private readonly byte[] _emailKey;
    public UserService(IskenderContext context, byte[] emailKey)
    {
        _context = context;
        _emailKey = emailKey;
    }
    
    public UserData GetUserById(ulong userId)
    {
        UserData? user = _context.UserData.Find(userId);
        return user;
    }
    
    public UserData GetUserByName(string userName)
    {
        UserData? user = _context.UserData.FirstOrDefault(u => u.UserName == userName);
        return user;
    }

    public UserData GetUserByEmail(string userEmail)
    {
        UserData? user = _context.UserData.FirstOrDefault(u => u.UserMail == userEmail);
        return user;
    }

 public IEnumerable<UserData> SearchUsers(string userName)
{
    UserData[] users = _context.UserData
        .Where(u => u.UserName.Contains(userName))
        .ToArray();

    foreach (var user in users)
    {
        user.UserPassword = null!;
    }

    return users;
}

    public bool ValidateUser(ulong userId, string userPassword)
    {
        UserData? user = _context.UserData.Find(userId);
        if (user == null)
        {
            return false;
        }
        return BCrypt.Net.BCrypt.Verify(userPassword, user.UserPassword);
    }

    public bool ValidateUser(string userName, string userPassword)
    {
        UserData? user = _context.UserData.FirstOrDefault(u => u.UserName == userName);
        if (user == null)
        {
            return false;
        }
        return BCrypt.Net.BCrypt.Verify(userPassword, user.UserPassword);
    }

    public bool ValidateUserByEmail(string email, string userPassword)
    {
        UserData? user = _context.UserData.FirstOrDefault(u => u.UserMail == email);
        if (user == null)
        {
            return false;
        }
        return BCrypt.Net.BCrypt.Verify(userPassword, user.UserPassword);
    }

    public string GenerateEmailVerification(string email)
    {
       throw new NotImplementedException();
    }

    public bool VerifyEmail(string email, string mailCode)
    {
        throw new NotImplementedException();
    }

    public bool AddUser(UserData user)
    {
        user.UserPassword = BCrypt.Net.BCrypt.HashPassword(user.UserPassword);
        _context.UserData.Add(user);
        return _context.SaveChanges() > 0;
    }
}