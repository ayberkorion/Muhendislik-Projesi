# Lojistik Yönetim Sistemi

Mühendislik projesi kapsamında geliştirdiğim bir nakliye ve lojistik yönetim uygulaması.

## Özellikler

- Kullanıcı girişi ve yetkilendirme
- Stok takibi ve depo yönetimi
- Gönderi oluşturma ve takibi
- Ürün ekleme ve düzenleme
- Kontrol paneli ile genel durum görüntüleme

## Teknolojiler

- C# Windows Forms
- .NET Framework 4.8
- SQL Server
- ADO.NET

## Kurulum

1. Projeyi klonlayın
2. SQL Server'da `NakliyeDB` adında veritabanı oluşturun
3. Gerekli tabloları oluşturun (Kullanicilar, Depolar, Urunler, Stok, Gonderiler)
4. `Form1.cs` dosyasındaki connection string'i güncelleyin
5. Projeyi Visual Studio'da açıp çalıştırın

## Veritabanı Tabloları

```sql
-- Kullanıcılar
CREATE TABLE Kullanicilar (
    KullaniciID INT PRIMARY KEY IDENTITY(1,1),
    AdSoyad NVARCHAR(100),
    KullaniciAdi NVARCHAR(50),
    Sifre NVARCHAR(100),
    Rol NVARCHAR(50)
);

-- Depolar
CREATE TABLE Depolar (
    DepoID INT PRIMARY KEY IDENTITY(1,1),
    DepoAdi NVARCHAR(100),
    Lokasyon NVARCHAR(200),
    Kapasite INT,
    MevcutStok INT
);

-- Ürünler
CREATE TABLE Urunler (
    UrunID INT PRIMARY KEY IDENTITY(1,1),
    UrunAdi NVARCHAR(100),
    Kategori NVARCHAR(50),
    BirimFiyat DECIMAL(10,2),
    Aciklama NVARCHAR(500)
);

-- Stok
CREATE TABLE Stok (
    StokID INT PRIMARY KEY IDENTITY(1,1),
    UrunID INT FOREIGN KEY REFERENCES Urunler(UrunID),
    DepoID INT FOREIGN KEY REFERENCES Depolar(DepoID),
    Miktar INT
);

-- Gönderiler
CREATE TABLE Gonderiler (
    GonderiID INT PRIMARY KEY IDENTITY(1,1),
    GonderiNo NVARCHAR(50),
    AliciAdi NVARCHAR(100),
    AliciAdresi NVARCHAR(500),
    GonderiTarihi DATETIME,
    TahminiTeslimTarihi DATETIME,
    Durum NVARCHAR(50),
    UrunID INT,
    Miktar INT
);
```
