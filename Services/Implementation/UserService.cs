using Microsoft.EntityFrameworkCore;
using ProjeIskender.Context;
using ProjeIskender.Models;
using ProjeIskender.Models.Dto;

namespace ProjeIskender.Services.Implementation;

public class UserService : IUserService
{
    private IskenderContext _context;
    public UserService(IskenderContext context)
    {
        _context = context;
    }
    
    public UserData GetUserById(ulong userId)
    {
        UserData? user = _context.UserData.Find(userId);
        if (user == null)
        {
            throw new Exception("Kullanıcı bulunamadı");
        }
        return user;
    }
    
    public UserData GetUserByName(string userName)
    {
        UserData? user = _context.UserData.FirstOrDefault(u => u.UserName == userName);
        if (user == null)
        {
            throw new Exception("Kullanıcı bulunamadı");
        }
        return user;
    }

    public UserData GetUserByEmail(string userEmail)
    {
        UserData? user = _context.UserData.FirstOrDefault(u => u.UserMail == userEmail);
        if (user == null)
        {
            throw new Exception("Kullanıcı bulunamadı");
        }
        return user;
    }

    public IEnumerable<UserData> SearchUsers(string userName)
    {
        return _context.UserData.Where(u => EF.Functions.ILike(u.UserName, userName + "%")).ToList();
    }

    public bool ValidateUser(ulong userId, string userPassword)
    {
        UserData? user = _context.UserData.Find(userId);
        if (user == null)
        {
            return false;
        }
        return user.UserPassword == userPassword;
    }

    public bool ValidateUser(string userName, string userPassword)
    {
        UserData? user = _context.UserData.FirstOrDefault(u => u.UserName == userName);
        if (user == null)
        {
            return false;
        }
        return user.UserPassword == userPassword;
    }

    public bool ValidateUserByEmail(string email, string userPassword)
    {
        UserData? user = _context.UserData.FirstOrDefault(u => u.UserMail == email);
        if (user == null)
        {
            return false;
        }
        return user.UserPassword == userPassword;
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
        _context.UserData.Add(user);
        return _context.SaveChanges() > 0;
    }

    public bool UpdateUserPicture(ulong userId, string pictureUrl)
    {
        UserData? user = _context.UserData.Find(userId);
        if (user == null)
        {
            return false;
        }
        user.PictureUrl = pictureUrl;
        _context.UserData.Update(user);
        return _context.SaveChanges() > 0;
    }
}