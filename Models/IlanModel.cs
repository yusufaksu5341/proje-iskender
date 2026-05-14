namespace ProjeIskender.Models;

public class IlanModel
{
    public int    Id           { get; set; }
    public string Baslik       { get; set; } = "";
    public string Kategori     { get; set; } = "";
    public string Sehir        { get; set; } = "";
    public string Icon         { get; set; } = "📦";
    public string Aciklama     { get; set; } = "";
    public string Durum        { get; set; } = "Artırma"; // "Artırma" | "Sabit"
    public string UrunDurumu   { get; set; } = "İyi";
    public decimal MevcutTeklif { get; set; }
    public decimal MinTeklif   { get; set; }
    public int    TeklifSayisi { get; set; }
    public int    KalanSaniye  { get; set; }
    public string Badge        { get; set; } = "new"; // hot | new | end
    public string SaticiAdi    { get; set; } = "";
    public string? MainImage   { get; set; } = null;
}
