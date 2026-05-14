using Microsoft.AspNetCore.Mvc;
using ProjeIskender.Models.Dto;
using ProjeIskender.Services;

namespace ProjeIskender.Controllers;

[Route("/account")]
public class Account : Controller
{
    private readonly IUserService _userService;

    public Account(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("login")]
    public IActionResult Login() => View();

    [HttpPost("login")]
    public IActionResult Login(string identifier, string password)
    {
        bool valid = identifier.Contains('@')
            ? _userService.ValidateUserByEmail(identifier, password)
            : _userService.ValidateUser(identifier, password);

        if (!valid)
        {
            ViewBag.Error = "Kullanıcı adı/e-posta veya şifre hatalı.";
            return View();
        }

        var user = identifier.Contains('@')
            ? _userService.GetUserByEmail(identifier)
            : _userService.GetUserByName(identifier);

        HttpContext.Session.SetString("Username", user!.UserName);
        HttpContext.Session.SetString("UserId", user.UserId.ToString());
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("register")]
    public IActionResult Register() => View();

    [HttpPost("register")]
    public IActionResult Register(string username, string email, string password)
    {
        if (_userService.GetUserByName(username) != null || _userService.GetUserByEmail(email) != null)
        {
            ViewBag.Error = "Bu kullanıcı adı veya e-posta zaten kullanılıyor.";
            return View();
        }

        var newUser = new UserData
        {
            UserName     = username,
            UserMail     = email,
            UserPassword = password,
        };

        if (!_userService.AddUser(newUser))
        {
            ViewBag.Error = "Kayıt sırasında bir hata oluştu. Lütfen tekrar deneyin.";
            return View();
        }

        HttpContext.Session.SetString("Username", username);
        HttpContext.Session.SetString("UserId", newUser.UserId.ToString());
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
