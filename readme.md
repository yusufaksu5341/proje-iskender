![ASP.NET](https://img.shields.io/badge/ASP.NET_Core_9-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![REST API](https://img.shields.io/badge/REST_API-000000?style=for-the-badge)
![JWT](https://img.shields.io/badge/Custom_JWT-orange?style=for-the-badge)
![MVC](https://img.shields.io/badge/MVC-5C2D91?style=for-the-badge)
# Proje İskender


İkinci el açık arttırmalı alım ve satım platformu.

## İçindekiler
- [Proje Hakkında](#proje-hakkında)
- [Özellikler](#özellikler)
- [Kullanılan Teknolojiler](#kullanılan-teknolojiler)
- [Kurulum](#kurulum)
- [Derleme](#derleme)
- [Sistem Mimarisi](#sistem-mimarisi)
- [API Mimarisi](#api-mimarisi)
- [Özelleştirilmiş JWT Doğrulama Sistemi](#özelleştirilmiş-jwt-doğrulama-sistemi)
- [Middleware](#middleware)
- [Test Süreçleri](#test-süreçleri)
- [Medya Yönetimi](#medya-yönetimi)
- [Geliştirici Ekip](#geliştirici-ekibi)


## Proje Hakkında

**İskender**, kullanıcıların oyun, elektronik, müzik, fotoğraf ekipmanları ve benzeri ürünler için ilan oluşturabildiği; bu ilanları açık artırma veya sabit fiyat sistemiyle yayınlayabildiği modern bir pazar yeri platformudur.

Platformun temel amacı:

- Güvenli açık artırma sistemi sunmak
- Gerçek zamanlı rekabet hissini artırmak
- Esnek ve ölçeklenebilir backend mimarisi oluşturmak
- RESTful API altyapısıyla farklı istemcilere kolay entegrasyon sağlamak

Sistem tamamen modern backend prensipleri üzerine kurulmuştur ve özellikle:

- Custom JWT Authentication
- Middleware tabanlı güvenlik sistemi
- Katmanlı mimari
- PostgreSQL + EF Core veri yönetimi
- Modüler REST API yapısı

gibi ileri seviye yazılım mimarileri kullanılmıştır.

---

## Özellikler

### Kullanıcı Sistemi
- Kullanıcı kayıt & giriş sistemi
- JWT tabanlı kimlik doğrulama
- Kullanıcı adı veya e-posta ile giriş
- Profil görüntüleme
- Kullanıcı arama sistemi
- Kullanıcı takip sistemi

### Açık Artırma & İlan Sistemi
- Açık artırmalı ürün ilanları
- Sabit fiyatlı satış sistemi
- Maksimum 1 aylık açık artırma süresi
- Dinamik teklif sistemi
- Ürün filtreleme
- Etiket bazlı keşif sistemi

### Medya Yönetimi
- Güvenli görsel yükleme sistemi
- MIME type doğrulama
- Resource yönetim sistemi
- Sunucu taraflı medya saklama

### Güvenlik
- Sıfırdan geliştirilmiş JWT altyapısı
- Custom Authentication Middleware
- Custom Authorization sistemi
- BCrypt ile parola hashleme
- Content-Type güvenlik kontrolü

---

## Kullanılan Teknolojiler

| Teknoloji                 | Açıklama             |
|---------------------------|----------------------|
| ASP.NET Core 9            | Backend framework    |
| Bootstrap 5               | Web Tasarımı         |
| MVC                       | Web mimarisi         |
| REST API                  | API iletişim sistemi |
| PostgreSQL                | Veritabanı           |
| Entity Framework Core     | ORM katmanı          |
| Npgsql                    | PostgreSQL provider  |
| BCrypt.Net                | Şifre hashleme       |
| JWT                       | Kimlik doğrulama     |
| Postman                   | API testleri         |


## Kurulum

### Repo Klonlama:
```bash
git clone https://github.com/yusufaksu5341/proje-iskender.git
cd proje-iskender
```

### PostgreSQL Kur

Postgre kurduktan sonra yeni bir veritabanı oluşturmak için:

```sql
CREATE DATABASE database-name;
```

---

### config.json Yapılandırması

config.json kendi database göre düzenleyin.

```json
{
  "ConnectionString": "Host=hostunuz;Database=veri-tabanı-ismi;Username=postgre-kullanıcı-adı;Password=şifreniz",
  "JwtKey": "32-karakterli-bir-şifre-belirleyiniz"
}
```

---

### Migration İşlemleri

```bash
dotnet ef database update
```


## Derleme
Projeyi çalıştırmak için:

```bash
dotnet run
```

Release build almak için:

```bash
dotnet build --configuration Release
```

Publish işlemi:

```bash
dotnet publish -c Release
```

## Sistem Mimarisi

Proje, sürdürülebilirlik ve test edilebilirlik için MVC mimarisi ile geliştirilmiştir.

```
Controllers
   ↓
Services
   ↓
Context (EF Core)
   ↓
PostgreSQL
```

### Katmanlar

#### Controllers
- REST endpoint yönetimi
- HTTP request handling
- MVC yönlendirmeleri

#### Services
- Business logic
- Veri yönetimi
- Dependency Injection yapısı

#### Middlewares
- Authentication
- Authorization
- Content validation

#### Context
- EF Core DbContext
- Entity yönetimi
- Veritabanı bağlantıları

---

## API Mimarisi

İskender backend sistemi tamamen RESTful prensiplere uygun şekilde geliştirilmiştir.  
API mimarisi; ölçeklenebilirlik, modülerlik ve farklı istemcilerle kolay entegrasyon hedeflenerek tasarlanmıştır.

Sistem, frontend katmanından bağımsız çalışabilecek şekilde organize edilmiştir. Bu sayede mevcut web arayüzüne ek olarak mobil uygulamalar veya farklı client yapıları backend sistemini doğrudan kullanabilir.

---

### API Tasarım Prensipleri

- REST tabanlı endpoint yapısı
- Durumsuz (stateless) istek modeli
- JSON tabanlı veri iletişimi
- Modüler endpoint ayrımı
- İstemciden bağımsız backend yapısı
- Katmanlı servis mimarisi
- Token tabanlı güvenli kimlik doğrulama

---

### Endpoint Dökümanı

Endpointlere ait detaylı dokümantasyona [ApiEndpoints.md](/Documents/ApiEndpoints.md) dosyası üzerinden ulaşabilirsiniz.


## Özelleştirilmiş JWT Doğrulama Sistemi

Projede hazır authentication paketleri yerine tamamen özelleştirilmiş JWT sistemi geliştirilmiştir.

Sistem:
- Token serialization
- Token validation
- Expiration control
- Role parsing
- Request authentication

işlemlerini tamamen proje içerisinde yönetmektedir.


## Middleware

Projede request pipeline yapısı özelleştirilmiş middleware sistemi ile yönetilmektedir.

Kullanılan middleware yapıları:
- Authentication Middleware
- Authorization Middleware
- ContentAccept Middleware

Bu yapı sayesinde:
- request validation
- authorization control
- content filtering

yüksek performanslı şekilde sağlanmaktadır.

## Test Süreçleri

Projede:
- Postman API testleri
- Custom test altyapısı
- Compiler directive bazlı test sistemi

kullanılmıştır.

### Test Senaryoları

- Geçerli/geçersiz JWT
- Hatalı giriş denemeleri
- Yetki kontrolü
- API response testleri
- İçerik doğrulama testleri

---

##  Medya Yönetimi

Yüklenen medya dosyaları:

```txt
/resource
```

dizini altında saklanır.

Sistem:
- MIME doğrulaması yapar
- Zararlı dosyaları engeller
- Dosya türlerini whitelist ile kontrol eder

---


## Geliştirici Ekibi

- Yusuf Enes Aksu
- Batuhan Torlak
- Halil Kaya

---
