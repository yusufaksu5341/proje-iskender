using Microsoft.AspNetCore.Mvc;
using ProjeIskender.Models;
using ProjeIskender.Services;

namespace ProjeIskender.Controllers;

[Route("/hesap")]
public class Profile : Controller
{
    private readonly IProductService _productService;

    public Profile(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("ilanlarim")]
    public IActionResult MyListings()
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr))
            return RedirectToAction("Login", "Account");

        ulong userId = ulong.Parse(userIdStr);

        var products = _productService.GetAllVisibleProducts()
            .Where(p => p.OwnerId == userId)
            .Select(p => new IlanModel
            {
                Id           = (int)p.ProductId,
                Baslik       = p.Name,
                Kategori     = Listing.GetDetail(p, "kategori") ?? "Genel",
                Sehir        = Listing.GetDetail(p, "sehir") ?? "Türkiye",
                Durum        = p.SinglePrice ? "Sabit" : "Artırma",
                MevcutTeklif = (decimal)p.CurrentPrice,
                TeklifSayisi = _productService.GetProductBidCount(p.ProductId),
                KalanSaniye  = (int)Math.Max(0, (p.ExpirationDate - DateTime.Now).TotalSeconds),
                Badge        = Listing.GetBadge(p),
                MainImage    = p.MainImage,
            }).ToList();

        return View(products);
    }

    [HttpGet("tekliflerim")]
    public IActionResult MyBids()
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr))
            return RedirectToAction("Login", "Account");

        ulong userId = ulong.Parse(userIdStr);

        var bids = _productService.GetUserBids(userId).ToList();

        var bidModels = new List<BidViewModel>();
        foreach (var b in bids)
        {
            try
            {
                var p = _productService.GetProduct(b.ProductId);
                if (p.SinglePrice) continue;
                bidModels.Add(new BidViewModel
                {
                    BidPrice = b.Price,
                    BidDate  = b.BidDate,
                    Ilan     = new IlanModel
                    {
                        Id           = (int)p.ProductId,
                        Baslik       = p.Name,
                        Kategori     = Listing.GetDetail(p, "kategori") ?? "Genel",
                        Durum        = "Artırma",
                        MevcutTeklif = (decimal)p.CurrentPrice,
                        KalanSaniye  = (int)Math.Max(0, (p.ExpirationDate - DateTime.Now).TotalSeconds),
                        Badge        = Listing.GetBadge(p),
                        MainImage    = p.MainImage,
                    }
                });
            }
            catch { }
        }

        return View(bidModels);
    }

    [HttpGet("alinanlarim")]
    public IActionResult Purchases()
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr))
            return RedirectToAction("Login", "Account");

        ulong userId = ulong.Parse(userIdStr);
        var now = DateTime.Now;

        var products = _productService.GetPurchases(userId)
            .Select(p => new IlanModel
            {
                Id           = (int)p.ProductId,
                Baslik       = p.Name,
                Kategori     = Listing.GetDetail(p, "kategori") ?? "Genel",
                Sehir        = Listing.GetDetail(p, "sehir") ?? "Türkiye",
                Durum        = p.SinglePrice ? "Sabit" : "Artırma",
                MevcutTeklif = (decimal)p.CurrentPrice,
                TeklifSayisi = _productService.GetProductBidCount(p.ProductId),
                KalanSaniye  = 0,
                Badge        = Listing.GetBadge(p),
                MainImage    = p.MainImage,
            }).ToList();

        return View(products);
    }

    [HttpGet("favorilerim")]
    public IActionResult Favorites()
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr))
            return RedirectToAction("Login", "Account");

        ulong userId = ulong.Parse(userIdStr);

        var products = _productService.GetFollowedProducts(userId)
            .Select(p => new IlanModel
            {
                Id           = (int)p.ProductId,
                Baslik       = p.Name,
                Kategori     = Listing.GetDetail(p, "kategori") ?? "Genel",
                Sehir        = Listing.GetDetail(p, "sehir") ?? "Türkiye",
                Durum        = p.SinglePrice ? "Sabit" : "Artırma",
                MevcutTeklif = (decimal)p.CurrentPrice,
                TeklifSayisi = _productService.GetProductBidCount(p.ProductId),
                KalanSaniye  = (int)Math.Max(0, (p.ExpirationDate - DateTime.Now).TotalSeconds),
                Badge        = Listing.GetBadge(p),
                MainImage    = p.MainImage,
            }).ToList();

        return View(products);
    }
}
