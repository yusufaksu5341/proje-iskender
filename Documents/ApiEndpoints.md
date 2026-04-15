# İçerik

- [Endpoint'ler](#endpointler)
    - [Account](#account-endpointleri)
      - [/api/account/login](#get-apiaccountlogin) | Kullanıcı girişi
      - [/api/account/register](#post-apiaccountregister) | Hesap oluşturma
    - [Product](#product-endpointleri)
      - [/api/product/search](#get-apiproductsearch) | Ürün aratma
      - [/api/user/search](#get-apiusersearch) | Kullanıcı aratma
    - [Resource](#resource-endpointleri)
- [Veri Tipleri](#veri-tipleri)
    - [Search-Respond](#search-respond) | [/api/search-prod](#get-apisearch-prod) dönüş değeri

# Endpoint'ler

# Account Endpoint'leri

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

## GET /api/account/{userId}

Kullanıcı profilinin verilerini döndürür

> NOT
> 
> Kullanıcının profil fotoğrafı da url olarak dönücek
> 
> userId gönderilmezse kullanıcının kendi profili açılacak

| Özellik | Değer |
|---|-------|
| Yetkilendirme | Hayır |
| Kimlik Doğrulama | Evet  |

## POST /api/account/{userId}/verify-email

Kullanıcının Email'ini doğrılamasını sağlar

> NOT
> 
> Doğrulama kodu URL üzerinden alınacak

| Özellik | Değer |
|---|-------|
| Yetkilendirme | Hayır |
| Kimlik Doğrulama | Hayır |

## GET /api/account/search

> NOT
>
> Kullanıcı adı üzerinden arıyacak o yüzden çok da uğraşmana gerek yok.
> 
> Url üzerinden göndericek sorguyu

| Özellik | Değer |
|---|-------|
| Yetkilendirme | Hayır |
| Kimlik Doğrulama | Evet  |

## PUT /api/account/{userId}/picture

> NOT
>
> KULLANICIYI KONTROL ETMEYİ UNUTMA kldsfjslkfjs

| Özellik | Değer |
|---|-------|
| Yetkilendirme | Hayır |
| Kimlik Doğrulama | Evet  |

# Product Endpoint'leri

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

## POST /api/product/{productId}/follow

## POST /api/product/{productId}/bid

# Resource Endpoint'leri

# Veri Tipleri

## Search-Respond

[/api/search-prod](#get-apisearch-prod) dönüş değeri

| isim            | Veri Tipi | Zorunlu  | Açıklama             |
|-----------------|-----------|----------|----------------------|
| id         | uint64    | Evet     | Ürün ID              |
| name       | char[256] | Evet     | Ürün adı             |
| date       | TimeSpan  | Evet     | Ürün yüklenme tarihi |
| last-price | float64   | Evet     | Ürünün son fiyatı    |
