# RedisApp Kullanım Kılavuzu

Bu uygulama, ASP.NET Core ile Redis kullanarak basit bir CRUD (Create, Read, Update, Delete) işlemleri uygulamasıdır. Redis, verilerin hafızada (memory) saklanmasını sağlayan bir veri depolama çözümüdür.

## Gereksinimler

- .NET 6.0 SDK
- Redis Server

## Kurulum

1. **Redis Server Kurulumu**:
   - Redis'i bilgisayarınıza kurun. Kurulum talimatları için [Redis.io](https://redis.io/download) adresini ziyaret edebilirsiniz.

2. **Uygulamanın Çalıştırılması**:
   - Proje dosyalarını indirin veya klonlayın.
   - Uygulamayı çalıştırmadan önce `appsettings.json` dosyasını düzenleyin ve Redis bağlantı bilgilerinizi ayarlayın.

## RedisApp Hakkında

### `Person` Sınıfı

Bu uygulama, `Person` adında basit bir sınıfı Redis'te saklamak için kullanır. Bu sınıf aşağıdaki özelliklere sahiptir:

- `Id`: Kişinin benzersiz kimliği. Redis'te anahtar olarak kullanılır.
- `FirstName`: Kişinin adı.
- `LastName`: Kişinin soyadı.
- `Age`: Kişinin yaşı.

### `PersonModel` Sınıfı

`Person` sınıfını ve bir süre (TTL) bilgisini içeren `PersonModel` sınıfı, kayıt ve güncelleme işlemleri için kullanılır.

### Ana Sayfa (`Index`)

Ana sayfa, temel bir görüntüleme sayfasıdır. Burada herhangi bir işlem yapılmaz, sadece sayfa gösterilir.

### Tüm Kullanıcıları Listele (`GetAll`)

Bu sayfa, Redis'te saklanan tüm kullanıcıları listeler.

### Yeni Kullanıcı Ekle (`Kaydet`)

Bu sayfa, yeni bir kullanıcı eklemek için kullanılır. Kullanıcı bilgileri Redis'e JSON formatında eklenir. Eğer `Ttl` değeri verilirse, kullanıcı bilgilerinin ne kadar süreyle saklanacağı belirlenir.

### Kullanıcı Güncelle (`Kaydet`)

Varolan bir kullanıcıyı güncellemek için kullanılır. Kullanıcıyı bulmak için `Id` kullanılır ve bilgileri güncellenir.

### Kullanıcı Sil (`RemoveItem`)

Bu sayfa, belirli bir kullanıcıyı Redis'ten silmek için kullanılır. Kullanıcıya ait `Id` değeri kullanılarak silme işlemi gerçekleştirilir.

## Nasıl Kullanılır?

1. Uygulamayı başlatın.
2. Ana sayfada mevcut kullanıcıları listeleyin.
3. Yeni kullanıcı eklemek için "Yeni Kullanıcı Ekle" sayfasını kullanın.
4. Varolan bir kullanıcıyı güncellemek için "Kullanıcı Güncelle" sayfasını kullanın.
5. Bir kullanıcıyı silmek için "Kullanıcı Sil" sayfasını kullanın.

Uygulama basit CRUD işlemlerini gerçekleştirebilir ve bu işlemleri Redis üzerinde saklar.

---

Bu `readme.md` dosyası, RedisApp uygulamasını kurmak, çalıştırmak ve temel kullanımı hakkında bilgi sağlar. Uygulamayı geliştirmek veya özelleştirmek için ilgili sınıfları inceleyebilir ve değişiklikler yapabilirsiniz.

Başka bir sorunuz veya yardıma ihtiyacınız olursa lütfen belirtin!
