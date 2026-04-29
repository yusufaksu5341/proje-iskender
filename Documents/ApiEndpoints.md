# İçerik

- [Endpoint'ler](#endpointler)
    - [/api/account/login](#get-apiaccountlogin) | Kullanıcı girişi
    - Product
        - [GET /api/product/{productId}](#get-apiproductproductid) | Ürün verisi çekme
        - [POST /api/product/create](#post-apiproductcreate) | Ürün oluşturma
        - [POST /api/product/{productId}/image](#post-apiproductproductidimage) | Ürün fotoğrafı ekleme
        - [PUT /api/product/{productId}/image/{resourcePath}](#put-apiproductproductidimageresourcepath) | Ürün kapak fotoğrafı değiştirme
        - [GET /api/product/search](#get-apiproductsearch) | Ürün aratma
        - [GET /api/product/{productId}/follow](#get-apiproductproductidfollow) | Ürünün takip edilip edilmediğini kontrol etme
        - [POST /api/product/{productId}/follow](#post-apiproductproductidfollow) | Ürün takip etme
        - [POST /api/product/{productId}/bid](#post-apiproductproductidbid) | Ürüne fiyat koyma
- [Veri Tipleri](#veri-tipleri)
    - [Search-Respond](#search-respond) | [/api/search-prod](#get-apisearch-prod) dönüş değeri

# Endpoint'ler

## GET /api/account/login

Kullanıcının giriş yapmasını sağlar

| Özellik | Değer  |
|---|--------|
| Yetkilendirme | Hayır  |
| Kimlik Doğrulama | Hayır  | 

### Parametreler:
`Accept: application/json`

| isim     | Veri Tipi                        | Zorunlu | Açıklama                   |
|----------|----------------------------------|---------|----------------------------|
| Name     | char[128]                        | Evet    | Kullanıcı ismi veya mail'i |
| Password | char[256]                        | Evet    | Kullanıcı şifresi          |
| NameType | uint8`[ EMail(0), UserName(1) ]` | Evet    | Name parametresinin türü   |

### Dönüş Değeri:
`application/jwt`

### Örnek:

Request:
```http request
GET /api/account/login
...
Content-Type: application/json
Content-Length: ...

{
    "Name": "test@mail.com",
    "Password": "12345",
    "NameType": 0
}
```

Respond:
```
HTTP/1.1 200 OK
Content-Type: application/jwt
Content-Length: ...

Ornek.Jwt.Tokeni
```

## POST /api/account/register

Kullanıcının kayıt oluşturmasını sağlar

| Özellik | Değer  |
|---|--------|
| Yetkilendirme | Hayır  |
| Kimlik Doğrulama | Hayır  | 

### Parametreler:
`Accept: application/json`

| isim           | Veri Tipi                        | Zorunlu | Açıklama                   |
|----------------|----------------------------------|---------|----------------------------|
| UserName       | char[128]                        | Evet    | Kullanıcı ismi             |
| UserMail       | char[256]                        | Evet    | Kullanıcı Maili            |
| UserPassword   | char[256]                        | Evet    | Kullanıcının Şifresi       |
| ConfirmPassword| char[256]                        | Evet    | Şifre Tekrarı              |
### Dönüş Değeri:
`text/plain`

### Örnek:

Request:
```http request
POST /api/account/register
...
Content-Type: application/json
Content-Length: ...

{
  "UserName": "Hu Tao",
  "UserPassword": "HuTaoLover",
  "ConfirmPassword": "HuTaoLover",
  "UserMail": "hutao@pablo.com"
}
```

Respond:
```
HTTP/1.1 200 OK
Content-Type: text/plain;
Content-Length: ...

Kullanıcı başarıyla oluşturuldu!
```

## POST /api/account/{userId}/validate-email/{validationCode}

## GET /api/product/{productId}

Ürün sayfasını döndürür

| Özellik | Değer |
|---|-------|
| Yetkilendirme | Hayır |
| Kimlik Doğrulama | Evet  |

### Parametreler:

[BOŞ]

### Dönüş Değeri:
`application/json`

> TODO
> 
> Buraya düzgün bir tablo getir

Detaylar için ProjeIskender.Models.Product.GetProductResult.cs dosyasına bakın

## POST /api/product/create

Kullanıcının ürün oluşturabilmesini sağlar. Görseller harici bir şekilder [POST /api/product/image](#post-apiproductproductidimage) endpoint'i üzerinden gönderilmelidir. Kapak fotoğrafı için [PUT /api/product/{productId}/image/{imagePath}](#put-apiproductproductidimageresourcepath)

| Özellik | Değer |
|---|-------|
| Yetkilendirme | Hayır |
| Kimlik Doğrulama | Evet  |

### Parametreler:
`Accept: application/json`

| isim         | Veri Tipi | Zorunlu | Varsayılan | Açıklama                                                                     |
|--------------|-----------|---------|------------|------------------------------------------------------------------------------|
| name         | char[128] | Evet    | Yok        | Ürün adı                                                                     |
| price        | float64   | Evet    | Yok        | Ürünün başlangıç fiyatını belirtir                                           |
| single-price | bool      | Evet    | Yok        | Ürünün tek fiyatlı mı yoksa açık arttırma mı olacağını belirler              |
| expire       | DateTime? | Evet    | Null       | Ürünün son geçerlilik tarihi. Eğer tek fiyatsa tarih belirtilmesine gerek yok |
| details      | Json?     | Hayır   | Null       | Ürün hakkında kuralsız detaylar listesi                                      |


### Dönüş Değeri:
`plain/text`

Oluşturulan ürünün id değerini döndürür

## POST /api/product/{productId}/image

Ürüne görsel eklemeyi sağlar.

| Özellik | Değer |
|---|-------|
| Yetkilendirme | Hayır |
| Kimlik Doğrulama | Evet  |

### Parametreler:
`Accept: Whitelist Dosya Formatları`

Whitelist'deki görsel formatlarını destekler

### Dönüş Değeri:
`plain/text`

Kaynağın api uzantısını döndürür

## PUT /api/product/{productId}/image/{resourcePath}

> NOT
> 
> resourcePath değişkeni "resource/gorsel-yolu" formatında olmalıdır
 
Ürünün kapak görselini değiştirir

| Özellik | Değer |
|---|-------|
| Yetkilendirme | Hayır |
| Kimlik Doğrulama | Evet  |

### Parametreler:

[BOŞ]

### Dönüş Değeri:

[BOŞ]

[Başarılı durumda 202-OK döndürür]

## GET /api/product/search

Kullanıcının aradığı ürünleri yirmişer sayfalar olarak döndürür.

| Özellik | Değer |
|---|-------|
| Yetkilendirme | Hayır |
| Kimlik Doğrulama | Evet  |

### Parametreler:
`Accept: application/x-www-form-urlencoded`

| isim     | Veri Tipi                 | Zorunlu | Varsayılan | Açıklama                                                                             |
|----------|---------------------------|---------|------------|--------------------------------------------------------------------------------------|
| name     | char[128]                 | Hayır   | `""`       | Ürün adı                                                                             |
| order    | `["asc", "desc", "rand"]` | Hayır   | `"rand"`   | Sıralama türü. Eğer `"rand"` kullanılırsa `name` ve `order-by` geçersiz sayılacaktır |
| order-by | `["price", "date"]`       | Hayır   | `"date"`   | Sıralama koşulu                                                                      |
| page     | uint32                    | Hayır   | `0`        | Getirilecek sayfa                                                                    |

### Dönüş Değeri:
`application/json`

| isim    | Veri Tipi               | Zorunlu | Varsayılan | Açıklama                                                               |
|---------|-------------------------|---------|------------|------------------------------------------------------------------------|
| count   | byte                    | Hayır   | `20`       | Ürün sayısı. Eğer son sayfa değilse otomatik olarak 20 döndürülmelidir |
| prods   | Search-Respond[`count`] | Evet    | Yok        | Ürünler                                                                |

### Örnek:

İstek:
```http request
GET /api/product/search
...
Content-Type: application/x-www-form-urlencoded
Content-Length: 23

name=iskender&order=asc
```

Dönüş:
> Burası henüz tamamlanmadı!
```
HTTP/1.1 200 OK
Content-Type: application/json
Content-Length: ...

{
    "count": 2,
    "prods": [
        {
            "
        }
    ]
}
```

## GET /api/product/{productId}/follow

Kullanıcının ürünü takip edip etmediğini gösterir

| Özellik | Değer |
|---|-------|
| Yetkilendirme | Hayır |
| Kimlik Doğrulama | Evet  |

### Parametreler:

[BOŞ]

### Dönüş Değeri:
`Content-Type: text/plain`

Eğer takip ediliyorsa `true`, aksi halde `false` metni döndürür

## POST /api/product/{productId}/follow

Kullanıcı eğer takip ediyorsa takipten çıkarır, eğer takip etmiyorsa takip eder
r
| Özellik | Değer |
|---|-------|
| Yetkilendirme | Hayır |
| Kimlik Doğrulama | Evet  |

### Parametreler:

[BOŞ]

### Dönüş Değeri:

[BOŞ]

## POST /api/product/{productId}/bid

Kullanıcının ürüne fiyat artırma yapmasını sağlar

| Özellik | Değer |
|---|-------|
| Yetkilendirme | Hayır |
| Kimlik Doğrulama | Evet  |

### Parametreler:
`Accept: application/x-www-form-urlencoded`

| isim  | Veri Tipi | Zorunlu | Varsayılan | Açıklama       |
|-------|-----------|---------|------------|----------------|
| price | float32   | Evet    | YOK        | Artırma değeri |

### Dönüş Değeri:

[BOŞ]

[Başarılı durumda 202-OK döndürür]

## POST /api/product/{productId}/buy

Kullanıcının ürünü satın almasını sağlar

| Özellik | Değer |
|---|-------|
| Yetkilendirme | Hayır |
| Kimlik Doğrulama | Evet  |

### Parametreler:

[BOŞ]

### Dönüş Değeri:

[BOŞ]

[Başarılı durumda 202-OK döndürür]

## GET /api/user/search

# Veri Tipleri

## Search-Respond

[/api/product/search](#get-apiproductsearch) dönüş değeri

| isim            | Veri Tipi | Zorunlu  | Açıklama             |
|-----------------|-----------|----------|----------------------|
| id         | uint64    | Evet     | Ürün ID              |
| name       | char[256] | Evet     | Ürün adı             |
| date       | TimeSpan  | Evet     | Ürün yüklenme tarihi |
| last-price | float64   | Evet     | Ürünün son fiyatı    |
