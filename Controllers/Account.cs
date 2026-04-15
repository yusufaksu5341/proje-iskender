using Microsoft.AspNetCore.Mvc;
using ProjeIskender.Models;

namespace ProjeIskender.Controllers;

[Route("/account")]
public class Account : Controller
{
    private static readonly List<AppUser> _users = new()
    {
        new AppUser("admin", "admin@iskender.com", "admin1234"),
    };

    [HttpGet("login")]
    public IActionResult Login() => View();

    [HttpPost("login")]
    public IActionResult Login(string identifier, string password)
    {
        var user = _users.FirstOrDefault(u =>
            (u.Username == identifier || u.Email == identifier) &&
            u.Password == password);

        if (user is null)
        {
            ViewBag.Error = "Kullanıcı adı/e-posta veya şifre hatalı.";
            return View();
        }

        HttpContext.Session.SetString("Username", user.Username);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("register")]
    public IActionResult Register() => View();

    [HttpPost("register")]
    public IActionResult Register(string username, string email, string password)
    {
        if (_users.Any(u => u.Username == username || u.Email == email))
        {
            ViewBag.Error = "Bu kullanıcı adı veya e-posta zaten kullanılıyor.";
            return View();
        }

        _users.Add(new AppUser(username, email, password));
        HttpContext.Session.SetString("Username", username);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
