PublicHolidayTracker – Türkiye Resmi Tatil Takip Sistemi 🇹🇷

PublicHolidayTracker, Görsel Programlama dersi kapsamında dönem ödevi olarak geliştirilmiş bir C# konsol uygulamasıdır. Uygulama; Nager.Date API üzerinden 2023, 2024 ve 2025 yıllarına ait Türkiye resmi tatillerini çekerek kullanıcıya yıl, tarih veya isim bazlı arama yapma imkânı sunar. Projenin temel amacı, modern C# tekniklerini kullanarak API tabanlı veri alma, bu veriyi OOP prensipleri doğrultusunda modelleme ve etkileşimli bir konsol arayüzü tasarlama deneyimi kazandırmaktır.

Bu kapsamda uygulama; API üzerinden elde edilen JSON verisini deserialize ederek nesnelere dönüştürür, kullanıcıyla etkileşim sağlayan bir menü sunar ve farklı filtreleme seçenekleri ile resmi tatilleri listeler. Veri alma işlemleri async/await yapısı ile asenkron şekilde gerçekleştirilerek uygulamanın donmasının önüne geçilmiştir. API’den gelen alan adlarında büyük/küçük harf uyumsuzluğu yaşanmaması için PropertyNameCaseInsensitive = true ayarı etkinleştirilmiştir. Ayrıca, kullanıcıların tarih girerken farklı formatlar kullanabileceği göz önünde bulundurularak akıllı bir tarih işleme algoritması geliştirilmiştir. Kullanıcıdan alınan tarih girdisi standart hale getirilerek gün ve ay değerleri güvenilir biçimde ayrıştırılmaktadır.

API tarafından dönen JSON verisinde fixed adında bir alan bulunduğundan, bu kelimenin C# dilinde rezerve edilmiş bir anahtar kelime olması sebebiyle doğabilecek çakışma @fixed şeklinde tanımlanarak çözülmüştür. Bu sayede model ile API uyumu korunmuş ve C# derleyicisiyle çelişmeyen bir yapı elde edilmiştir. Nullable tipler kullanılarak API’den boş gelebilecek verilere karşı hata koruması sağlanmıştır.

Uygulamanın geliştirilmesinde Visual Studio Community 2026 ortamı, C# (.NET 8), HttpClient ile asenkron veri çekme, System.Text.Json kütüphanesi ile JSON çözümleme, LINQ ile veri sorgulama ve List<T> gibi generic koleksiyon yapıları kullanılmıştır. Bu teknolojiler sayesinde kod yapısı hem sade hem de genişletilebilir bir mimariye sahiptir.

Projede kullanılan temel sınıf yapısı şu şekildedir:

public class Holiday
{
    public string? date { get; set; }        
    public string? localName { get; set; }   
    public string? name { get; set; }        
    public string? countryCode { get; set; } 
    public bool @fixed { get; set; }         
    public bool global { get; set; }         
}


Uygulama başlatıldığında kullanıcıyı aşağıdaki menü karşılar:

===== PublicHolidayTracker =====
1. Tatil listesini göster (Yıl Seçmeli - 2023/24/25)
2. Tarihe göre tatil ara (Akıllı Arama: gg-aa)
3. İsme göre tatil ara (Örn: Cumhuriyet)
4. Tüm tatilleri 3 yıl boyunca göster (2023–2025)
5. Çıkış


Uygulamayı çalıştırmak için projenin kaynak dosyaları indirilip Visual Studio Community 2026 ile açılır, internet bağlantısının aktif olduğundan emin olunur ve F5 tuşu ile uygulama derlenip çalıştırılır. API’den veriler anlık olarak alındığı için çalıştırma sırasında internet zorunludur.

Bu proje, C# ile API tabanlı veri işleme mantığını pekiştirmek ve konsol uygulamalarında kullanıcı deneyimini güçlendirmek amacıyla hazırlanmıştır. Kod yapısı modülerdir ve farklı yıl aralıkları, ek filtreleme seçenekleri veya veri kaydetme özellikleri gibi geliştirmelere açıktır.

meriç aydemir 20230108049
