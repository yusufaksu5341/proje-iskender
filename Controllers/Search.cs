using Microsoft.AspNetCore.Mvc;
using ProjeIskender.Models;
using ProjeIskender.Services;

namespace ProjeIskender.Controllers;

[Route("/arama")]
public class Search : Controller
{
    private readonly IProductService _productService;

    public Search(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public IActionResult Index(string? q, string? kategori, string? sehir,
                               decimal? minFiyat, decimal? maxFiyat,
                               string? tur, string? siralama)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            return RedirectToAction("Login", "Account");

        var allProducts = _productService.GetAllVisibleProducts().ToList();

        var filtered = allProducts
            .Where(p => string.IsNullOrEmpty(q) || p.Name.Contains(q, StringComparison.OrdinalIgnoreCase))
            .Where(p => string.IsNullOrEmpty(kategori) || Listing.GetDetail(p, "kategori") == kategori)
            .Where(p => string.IsNullOrEmpty(sehir) || Listing.GetDetail(p, "sehir") == sehir)
            .Where(p => minFiyat == null || (decimal)p.CurrentPrice >= minFiyat)
            .Where(p => maxFiyat == null || (decimal)p.CurrentPrice <= maxFiyat)
            .Where(p => string.IsNullOrEmpty(tur) ||
                        (tur == "artirma" && !p.SinglePrice) ||
                        (tur == "sabit" && p.SinglePrice));

        filtered = siralama switch
        {
            "fiyat-artan"  => filtered.OrderBy(p => p.CurrentPrice),
            "fiyat-azalan" => filtered.OrderByDescending(p => p.CurrentPrice),
            "teklif"       => filtered.OrderByDescending(p => _productService.GetProductBidCount(p.ProductId)),
            _              => filtered.OrderByDescending(p => p.CreationDate),
        };

        var results = filtered.Select(p => new IlanModel
        {
            Id           = (int)p.ProductId,
            Baslik       = p.Name,
            Kategori     = Listing.GetDetail(p, "kategori") ?? "Genel",
            Sehir        = Listing.GetDetail(p, "sehir") ?? "Türkiye",
            Durum        = p.SinglePrice ? "Sabit" : "Artırma",
            MevcutTeklif = (decimal)p.CurrentPrice,
            MinTeklif    = (decimal)(p.CurrentPrice + 1),
            TeklifSayisi = _productService.GetProductBidCount(p.ProductId),
            KalanSaniye  = (int)Math.Max(0, (p.ExpirationDate - DateTime.Now).TotalSeconds),
            Badge        = Listing.GetBadge(p),
            MainImage    = p.MainImage,
        }).ToList();

        ViewBag.Q        = q;
        ViewBag.Kategori = kategori;
        ViewBag.Sehir    = sehir;
        ViewBag.MinFiyat = minFiyat;
        ViewBag.MaxFiyat = maxFiyat;
        ViewBag.Tur      = tur;
        ViewBag.Siralama = siralama;

        return View(results);
    }
}
