using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjeIskender.Models;
using System.Linq;

namespace ProjeIskender.Controllers;

[Route("/")]
[Route("/home")]
public class Home : Controller
{
    private readonly ILogger<Home> _logger;

    public Home(ILogger<Home> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            return RedirectToAction("Login", "Account");

        return View(IlanStore.Ilanlar.ToList());
    }

    [HttpGet("privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpGet("error")]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
