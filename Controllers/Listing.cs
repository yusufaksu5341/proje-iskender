using Microsoft.AspNetCore.Mvc;
using ProjeIskender.Models;

namespace ProjeIskender.Controllers;

[Route("/ilan")]
public class Listing : Controller
{
    [HttpGet("ekle")]
    public IActionResult Add() => View();

    [HttpPost("ekle")]
    public IActionResult Add(string baslik, string kategori, string sehir,
                             string durum, string aciklama, decimal fiyat)
    {
        var username = HttpContext.Session.GetString("Username") ?? "anonim";

        IlanStore.Add(new IlanModel
        {
            Baslik       = baslik,
            Kategori     = kategori,
            Sehir        = sehir,
            UrunDurumu   = durum,
            Aciklama     = aciklama,
            MevcutTeklif = fiyat,
            MinTeklif    = fiyat + 50,
            Icon         = "📦",
            Durum        = "Artırma",
            KalanSaniye  = 7 * 24 * 3600,
            Badge        = "new",
            SaticiAdi    = username,
            TeklifSayisi = 0,
        });

        return RedirectToAction("Index", "Home");
    }

    [HttpGet("{id:int}")]
    public IActionResult Detail(int id)
    {
        var ilan = IlanStore.GetById(id);
        if (ilan is null) return NotFound();
        return View(ilan);
    }
}
