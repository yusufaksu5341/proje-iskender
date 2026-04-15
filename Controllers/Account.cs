using Microsoft.AspNetCore.Mvc;

namespace ProjeIskender.Controllers;

[Route("/account")]
public class Account : Controller
{
    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }
}
