using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ProjeIskender.Models;
using ProjeIskender.Models.Dto;
using ProjeIskender.Services;

namespace ProjeIskender.Controllers;

[Route("/ilan")]
public class Listing : Controller
{
    private readonly IProductService _productService;
    private readonly IResourceService _resourceService;

    public Listing(IProductService productService, IResourceService resourceService)
    {
        _productService = productService;
        _resourceService = resourceService;
    }

    [HttpGet("ekle")]
    public IActionResult Add()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            return RedirectToAction("Login", "Account");
        return View();
    }

    [HttpPost("ekle")]
    public IActionResult Add(string baslik, string kategori, string sehir,
                             string durum, string aciklama, decimal fiyat,
                             string ilanTuru, DateTime? bitisZamani,
                             List<IFormFile>? gorseller)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr))
            return RedirectToAction("Login", "Account");

        ulong userId = ulong.Parse(userIdStr);
        bool isSinglePrice = ilanTuru != "artirma";

        var expiration = isSinglePrice
            ? DateTime.Now.AddDays(30)
            : (bitisZamani.HasValue && bitisZamani.Value > DateTime.Now
                ? bitisZamani.Value
                : DateTime.Now.AddDays(7));

        var details = JsonSerializer.SerializeToElement(new
        {
            kategori, sehir, aciklama, urunDurumu = durum
        });

        var product = new ProductData
        {
            OwnerId        = userId,
            CreationDate   = DateTime.Now,
            ExpirationDate = expiration,
            Name           = baslik,
            Visible        = true,
            StartingPrice  = (float)fiyat,
            CurrentPrice   = (float)fiyat,
            SinglePrice    = isSinglePrice,
            Details        = details,
        };

        var productId = _productService.CreateProduct(product);

        if (gorseller != null)
        {
            bool firstImage = true;
            foreach (var file in gorseller.Where(f => f.Length > 0))
            {
                try
                {
                    using var stream = file.OpenReadStream();
                    var resourcePath = _resourceService.CreateResource(file.ContentType, stream);
                    _productService.AddProductImage(productId, resourcePath);
                    if (firstImage)
                    {
                        _productService.SetMainImage(productId, userId, resourcePath);
                        firstImage = false;
                    }
                }
                catch { }
            }
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet("{id:int}")]
    public IActionResult Detail(int id)
    {
        ProductData product;
        try { product = _productService.GetProduct((ulong)id); }
        catch { return NotFound(); }

        var userIdStr = HttpContext.Session.GetString("UserId");
        ViewBag.CurrentUserId = userIdStr;

        if (!string.IsNullOrEmpty(userIdStr))
        {
            ulong userId = ulong.Parse(userIdStr);
            ViewBag.IsOwner      = product.OwnerId == userId;
            ViewBag.IsFavorited  = _productService.IsFollowed(userId, product.ProductId);
        }
        else
        {
            ViewBag.IsOwner     = false;
            ViewBag.IsFavorited = false;
        }

        ViewBag.BidHistory = _productService.GetBidHistory(product.ProductId).ToList();
        ViewBag.Comments   = _productService.GetComments(product.ProductId).ToList();

        return View(ToIlanModel(product));
    }

    [HttpGet("{id:int}/fiyat")]
    public IActionResult GetCurrentPrice(int id)
    {
        try
        {
            var p = _productService.GetProduct((ulong)id);
            return Json(new { price = p.CurrentPrice, bidCount = _productService.GetProductBidCount((ulong)id) });
        }
        catch
        {
            return Json(new { price = 0, bidCount = 0 });
        }
    }

    [HttpPost("{id:int}/yorum")]
    [Microsoft.AspNetCore.Mvc.IgnoreAntiforgeryToken]
    public IActionResult AddYorum(int id, [FromBody] YorumRequest? req)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr))
            return Json(new { success = false, message = "Giriş yapmanız gerekiyor." });

        if (req == null || string.IsNullOrWhiteSpace(req.Icerik))
            return Json(new { success = false, message = "Yorum boş olamaz." });

        if (req.Icerik.Length > 500)
            return Json(new { success = false, message = "Yorum en fazla 500 karakter olabilir." });

        ulong userId = ulong.Parse(userIdStr);
        try { _productService.GetProduct((ulong)id); }
        catch { return Json(new { success = false, message = "İlan bulunamadı." }); }

        _productService.AddComment((ulong)id, userId, req.Icerik.Trim());
        var username = HttpContext.Session.GetString("Username") ?? "?";
        return Json(new { success = true, username, icerik = req.Icerik.Trim() });
    }

    [HttpPost("{id:int}/favori")]
    public IActionResult FavoriToggle(int id)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr))
            return Json(new { success = false, message = "Giriş yapmanız gerekiyor." });

        ulong userId = ulong.Parse(userIdStr);
        _productService.FollowProduct(userId, (ulong)id);
        bool nowFav = _productService.IsFollowed(userId, (ulong)id);
        return Json(new { success = true, isFavorited = nowFav });
    }

    [HttpPost("{id:int}/teklif")]
    [Microsoft.AspNetCore.Mvc.IgnoreAntiforgeryToken]
    public IActionResult Teklif(int id, [FromBody] TeklifRequest? req)
    {
        try
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr))
                return Json(new { success = false, message = "Giriş yapmanız gerekiyor." });

            if (req == null)
                return Json(new { success = false, message = "Geçersiz istek verisi." });

            ulong userId    = ulong.Parse(userIdStr);
            ulong productId = (ulong)id;

            ProductData product;
            try { product = _productService.GetProduct(productId); }
            catch { return Json(new { success = false, message = "İlan bulunamadı." }); }

            if (product.OwnerId == userId)
                return Json(new { success = false, isOwner = true, message = "Kendi ilanınıza teklif veremezsiniz." });

            if (product.SinglePrice)
            {
                bool bought = _productService.MakePurchase(userId, productId);
                if (!bought)
                    return Json(new { success = false, message = "Bu ilan artık satın alınamıyor." });
                return Json(new { success = true, isPurchase = true, message = "Satın alındı!" });
            }

            var latestBidder = _productService.GetLatestBidder(productId);
            if (latestBidder == userId)
                return Json(new { success = false, alreadyHighest = true, message = "Son teklifi zaten siz verdiniz!" });

            bool ok = _productService.MakeBid(userId, productId, req.Fiyat);
            if (!ok)
                return Json(new { success = false, message = $"Teklifiniz (₺{req.Fiyat:N0}) mevcut fiyattan düşük veya eşit olamaz." });

            int newCount = _productService.GetProductBidCount(productId);
            return Json(new { success = true, isPurchase = false, message = "Teklif verildi!", newPrice = req.Fiyat, bidCount = newCount });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Sunucu hatası: " + ex.Message });
        }
    }

    [HttpGet("{id:int}/duzenle")]
    public IActionResult Edit(int id)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr))
            return RedirectToAction("Login", "Account");

        ulong userId = ulong.Parse(userIdStr);
        ProductData product;
        try { product = _productService.GetProduct((ulong)id); }
        catch { return NotFound(); }

        if (product.OwnerId != userId)
            return Forbid();

        return View(ToIlanModel(product));
    }

    [HttpPost("{id:int}/duzenle")]
    public IActionResult Edit(int id, string baslik, string kategori, string sehir,
                              string durum, string aciklama, IFormFile? yeniGorsel)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr))
            return RedirectToAction("Login", "Account");

        ulong userId = ulong.Parse(userIdStr);
        ProductData product;
        try { product = _productService.GetProduct((ulong)id); }
        catch { return NotFound(); }

        if (product.OwnerId != userId)
            return Forbid();

        var details = JsonSerializer.SerializeToElement(new
        {
            kategori, sehir, aciklama, urunDurumu = durum
        });
        _productService.UpdateProductInfo((ulong)id, baslik, details);

        if (yeniGorsel != null && yeniGorsel.Length > 0)
        {
            try
            {
                using var stream = yeniGorsel.OpenReadStream();
                var resourcePath = _resourceService.CreateResource(yeniGorsel.ContentType, stream);
                _productService.AddProductImage((ulong)id, resourcePath);
                _productService.SetMainImage((ulong)id, userId, resourcePath);
            }
            catch { }
        }

        return RedirectToAction("Detail", new { id });
    }

    private IlanModel ToIlanModel(ProductData p) => new IlanModel
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
    };

    internal static string? GetDetail(ProductData p, string key)
    {
        if (p.Details == null) return null;
        if (p.Details.Value.ValueKind != JsonValueKind.Object) return null;
        if (p.Details.Value.TryGetProperty(key, out var val))
            return val.GetString();
        return null;
    }

    internal static string GetBadge(ProductData p)
    {
        var remaining = (p.ExpirationDate - DateTime.Now).TotalSeconds;
        if (remaining < 86400) return "end";
        if ((DateTime.Now - p.CreationDate).TotalDays < 2) return "new";
        return "hot";
    }
}

public class TeklifRequest
{
    [System.Text.Json.Serialization.JsonPropertyName("fiyat")]
    public float Fiyat { get; set; }
}

public class YorumRequest
{
    [System.Text.Json.Serialization.JsonPropertyName("icerik")]
    public string Icerik { get; set; } = "";
}
