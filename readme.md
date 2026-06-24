# proje-iskender

C# ile yazılmış bir ilan ve alım-satım platformu. Ürün ekleyip fotoğraf yükleyebilir, fiyat teklifi verebilirsin.

## Özellikler

- Üyelik ve token tabanlı oturum
- Ürün oluşturma ve fotoğraf yönetimi
- Ürün arama
- Ürün takip etme
- Fiyat teklifi verme

## Stack

ASP.NET Core MVC, Entity Framework Core, MSSQL

## Kurulum

`appsettings.json` içindeki bağlantı bilgilerini düzenle:

```bash
dotnet ef database update
dotnet run
```
