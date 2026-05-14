using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ProjeIskender.Models;
using ProjeIskender.Models.Dto;
using ProjeIskender.Services;

namespace ProjeIskender.Controllers;

[Route("/")]
[Route("/home")]
public class Home : Controller
{
    private readonly ILogger<Home> _logger;
    private readonly IProductService _productService;

    public Home(ILogger<Home> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            return RedirectToAction("Login", "Account");

        var products = _productService.GetProducts("", 0, QueryOrder.Descending, QueryType.Date).ToList();

        var ilanlar = products.Select(p => new IlanModel
        {
            Id           = (int)p.ProductId,
            Baslik       = p.Name,
            Kategori     = GetDetail(p, "kategori") ?? "Genel",
            Sehir        = GetDetail(p, "sehir") ?? "Türkiye",
            Icon         = "📦",
            Aciklama     = GetDetail(p, "aciklama") ?? "",
            Durum        = p.SinglePrice ? "Sabit" : "Artırma",
            UrunDurumu   = GetDetail(p, "urunDurumu") ?? "Belirtilmemiş",
            MevcutTeklif = (decimal)p.CurrentPrice,
            MinTeklif    = (decimal)(p.CurrentPrice + 1),
            TeklifSayisi = _productService.GetProductBidCount(p.ProductId),
            KalanSaniye  = (int)Math.Max(0, (p.ExpirationDate - DateTime.Now).TotalSeconds),
            Badge        = GetBadge(p),
            SaticiAdi    = "",
            MainImage    = p.MainImage,
        }).ToList();

        return View(ilanlar);
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

    private static string? GetDetail(ProductData p, string key)
    {
        if (p.Details == null) return null;
        if (p.Details.Value.ValueKind != System.Text.Json.JsonValueKind.Object) return null;
        if (p.Details.Value.TryGetProperty(key, out var val))
            return val.GetString();
        return null;
    }

    private static string GetBadge(ProductData p)
    {
        var remainingSecs = (p.ExpirationDate - DateTime.Now).TotalSeconds;
        if (remainingSecs < 86400) return "end";
        if ((DateTime.Now - p.CreationDate).TotalDays < 2) return "new";
        return "hot";
    }
}
