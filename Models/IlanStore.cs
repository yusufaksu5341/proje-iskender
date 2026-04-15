namespace ProjeIskender.Models;

public static class IlanStore
{
    private static int _nextId = 4;

    public static List<IlanModel> Ilanlar { get; } = new()
    {
        new IlanModel
        {
            Id           = 1,
            Baslik       = "PlayStation 5 Disk Edition — Kutu Açılmamış",
            Kategori     = "Oyun & Konsol",
            Sehir        = "İstanbul",
            Icon         = "",
            Durum        = "Artırma",
            UrunDurumu   = "Sıfır",
            MevcutTeklif = 18250,
            MinTeklif    = 18300,
            TeklifSayisi = 14,
            KalanSaniye  = 9653,
            Badge        = "hot",
            SaticiAdi    = "teknoloji_ustası",
            Aciklama     = "PlayStation 5 Disk Edition, kutu açılmamış orijinal ambalajında.\n\nHediye olarak alındı, zaten PS5 sahibi olduğum için satışa çıkarıyorum. Fatura ve tüm aksesuarlar kutusunda mevcuttur.\n\nKutu içeriği:\n• PS5 Konsol (Disk Sürümü)\n• DualSense Kablosuz Kumanda\n• HDMI Kablosu (4K)\n• USB Type-C Şarj Kablosu\n• AC Adaptör",
        },
        new IlanModel
        {
            Id           = 2,
            Baslik       = "Fender Stratocaster American Professional II",
            Kategori     = "Müzik Aletleri",
            Sehir        = "Ankara",
            Icon         = "",
            Durum        = "Artırma",
            UrunDurumu   = "İyi",
            MevcutTeklif = 28500,
            MinTeklif    = 28550,
            TeklifSayisi = 7,
            KalanSaniye  = 43200,
            Badge        = "new",
            SaticiAdi    = "muzisyen_88",
            Aciklama     = "2021 model Fender Stratocaster American Professional II. 3 ay kullanıldı, küçük bir kaç çizik dışında mükemmel durumda. Orijinal kılıfıyla birlikte.",
        },
        new IlanModel
        {
            Id           = 3,
            Baslik       = "Leica M11 Body — Az Kullanılmış",
            Kategori     = "Fotoğraf",
            Sehir        = "İzmir",
            Icon         = "",
            Durum        = "Artırma",
            UrunDurumu   = "Az Kullanılmış",
            MevcutTeklif = 95000,
            MinTeklif    = 95050,
            TeklifSayisi = 3,
            KalanSaniye  = 86400,
            Badge        = "end",
            SaticiAdi    = "fotograf_meraklisi",
            Aciklama     = "Leica M11 Body, 2022 üretim. 60MP tam kare sensör, OLED ekran. Aksesuarları ve orijinal kutusuyla birlikte. Fatura mevcut.",
        },
    };

    public static IlanModel? GetById(int id) =>
        Ilanlar.FirstOrDefault(i => i.Id == id);

    public static void Add(IlanModel ilan)
    {
        ilan.Id = _nextId++;
        Ilanlar.Add(ilan);
    }
}
