using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dongu : MonoBehaviour
{
    [Header("Zaman")] //Başlık belirlemede kullanıldı.
    [Tooltip("Dakikadaki gün uzunluğu")]
    [SerializeField] //Bu C dilindeki işaretçi gibidir.
    private float hedefGunUzunlugu = 0.5f; //Bu süratte gerçekleşecek.
    public float hedefUzunluk
    {
        get
        {
            return hedefGunUzunlugu;
        }
    }

    [SerializeField]
    private float zamanSarkmasi; //Hata payı gibi düşünülebilir.
    private bool yirmiDortSaat = true;
    [Range(0, 1)] //Sıfır ile bir arasında olacak.
    public float gunZamani = 2f;
    public float zaman
    {
        get
        {
            return gunZamani;
        }
    }
    public int yilSayisi; //Geçen yılın sayacını tutar.
    public int yil
    {
        get
        {
            return yilSayisi;
        }
    }
    public int gunSayisi;
    public int gun
    {
        get
        {
            return gunSayisi;
        }
    }
    private float zamanOlcusu = 100f;
    public float yilUzunlugu;
    public float uzunluk
    {
        get
        {
            return yilUzunlugu;
        }
    }
    public Text saatMetni;
    void Start()
    {

        OlaganZamanEgrisi(); //Aşağıda böyle bir method tanımlanacak.
    }
    public bool mola = false; //Ara vermeyi sağlamak adına kullanılır.
    [SerializeField]
    private AnimationCurve zamanEgrisi; //Bu zamanın değişimini gösterecek.
    public float zamanEgrisiNormallestirmesi;
    [Header("Güneş Işığı")] //Güneş ışığı başlığı altında olacak bölümler.
    public Transform gunlukDonme; //Burada döngü için nesne oluşturulabilir.
    [SerializeField] //Yine işaretçi kullanıldı.
    private Light gunes;//Işık kaynağı çağrıldı.
    private float yogunluk;//Işık yoğunluğunu belirlemek için kullanılacak değişkendir.
    [SerializeField]
    private float gunesTabanYogunlugu = 1f; //En düşük ışık yoğunluğu olabilir.
    [SerializeField]//Ne kadar çok işaretçi kullandık adrese erişebilmek için.
    private float gunesVaryasyonu = 1.5f; //Işık yoğunluğunun değişimini sağlayabilir.
    [SerializeField]
    private Gradient gunesRengi; //Akşam üstü kızıla dönecek gündüz sarı olacak.
    [Header("Mevsimsel Değişkenler")] //Mevsimsel değişkenler başlığı altında toplanacak.
    [SerializeField]
    private Transform gunesMevsimselDonus; //Yaz,kış,bahar ve güz döngüsü
    [SerializeField] //Yeniden işaretçi koyuyoruz.
    [Range(-45f, 45f)] //Bu dönüş alt sınırı sola 45 üst sınırı sağa 45 derece oluyor.
    private float enYuksekMevsimselEgim;
    [Header("Modüller")]
    private List<ModulTabani> modulListesi = new List<ModulTabani>(); //Evet ilklendirmeyi gerçekleştirdik.
    void Update()
    {
        if (!mola)
        {
            ZamanOlcusuGuncelle();
            zamanGuncelle();
            saatGuncelle();
        }
        gunesdonusAyari();
        gunesYogunlugu();
        gunesRenkAyari();
        //Her çerçeve için modül güncellenecek.

    }
    void ZamanOlcusuGuncelle()
    {
        zamanOlcusu = 24 / (hedefGunUzunlugu / 60);
        zamanOlcusu *= zamanEgrisi.Evaluate(zamanSarkmasi / (uzunluk * 60)); //Zaman eğrisi temel alınarak zaman ölçüsü değişimi yapılır.
        zamanOlcusu /= zamanEgrisiNormallestirmesi;//Zaman eğrisinin normal düzeye gelmesi amaçlanıyor.

    }
    void OlaganZamanEgrisi()
    {
        float adimBuyuklugu = 0.01f;
        int adimSayisi = Mathf.FloorToInt(1f / adimBuyuklugu); //Kendisinden küçük en büyük tam sayıya yuvarlama yapıldı.
        float toplamEgri = 0;
        for (int i = 0; i < adimSayisi; i++)
        {
            toplamEgri += zamanEgrisi.Evaluate(i * adimSayisi);
        }
        zamanEgrisiNormallestirmesi = toplamEgri / adimSayisi; //Gün uzunluğunu hedef değerde tutar.
    }
    void zamanGuncelle()
    {
        gunZamani += Time.deltaTime + zamanOlcusu / 86400; //Bir gündeki saniye miktarı.
        zamanSarkmasi += Time.deltaTime;
        if (gunSayisi > 1) //Yeni gün
        {
            zamanSarkmasi = 0f;

            gunSayisi++;//Gün sayısında bir artış olacak.
            gunZamani -= 1;//Gün zamanında bir azalış olacak.
            if (gunSayisi > yilUzunlugu) //Yeni yıl
            {
                yilSayisi++; //Yıl sayısına bir eklenir.
                gunSayisi = 0;//Gün sayısı yeniden başlar.
            }
        }
    }
    void saatGuncelle()
    {
        float zamani = zamanSarkmasi / (hedefGunUzunlugu / 60);//Zaman sarkmasının nedeni artık yılları da hesaba katmaktır.
        float saat = Mathf.FloorToInt(zamani * 24);
        float dakika = Mathf.FloorToInt(((zamani * 24) - saat) * 60);//Dakika hesabı da belirlendi.
        string saatDizgesi; //Saati string tipine dönüştüreceğiz.
        string dakikaDizgesi; //Dakikayı string tipine dönüştüreceğiz.
        if (!yirmiDortSaat && saat > 12)
        {
            saat -= 12;
        }
        if (saat < 10)
        {
            saatDizgesi = "0" + saat.ToString(); //Tek basamaklı sayıların başına sıfır ekledik.
        }
        else
        {
            saatDizgesi = saat.ToString();
        }
        if (dakika < 10)
        {
            dakikaDizgesi = "0" + dakika.ToString(); //Tek basamaklı sayıların başına sıfır ekledik.
        }
        else
        {
            dakikaDizgesi = dakika.ToString();
        }
        if (yirmiDortSaat)
        {
            saatMetni.text = saatDizgesi + ":" + dakikaDizgesi; //24 saat göstergesi

        }
        else if (zaman > 0.5f)
        {
            saatMetni.text = saatDizgesi + ":" + dakikaDizgesi + "p.m"; //Öğleden gece yarısına kadar olan süre

        }
        else
        {
            saatMetni.text = saatDizgesi + ":" + dakikaDizgesi + "p.m"; //Gece yarısından öğleye kadar olan süre.
        }

    }
    public void gunesdonusAyari()
    {
        float gunesAcisi = gunZamani * 360f; //Dairesel bir hareket olduğu için böyle.
        gunlukDonme.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, gunesAcisi));//Dönüş değeri ayarlandı.
        float mevsimselAci = -enYuksekMevsimselEgim * Mathf.Cos(gunSayisi / yilUzunlugu * 2f * Math.PI); //Açı ayarlandı.
        gunesMevsimselDonus.transform.localRotation = Quaternion.Euler(new Vector3(mevsimselAci, 0f, 0f));
    }
    public void gunesYogunlugu()
    {
        yogunluk = Vector3.Dot(gunes.transform.forward, Vector3.down); //Sayısal bir ifade çıkabilir.
        yogunluk = Mathf.Clamp01(yogunluk);
        gunes.intensity = yogunluk * gunesVaryasyonu * gunesTabanYogunlugu; //Güneş ışığının bağlı olduğu değişkenler.

    }
    public void gunesRenkAyari()
    {
        gunes.color = gunesRengi.Evaluate(yogunluk); //Işığın rengi de belirlendi.
    }
    public void Ekle(ModulTabani mt)
    {
        modulListesi.Add(mt);
    }
    public void Kaldir(ModulTabani mt)
    {
        modulListesi.Remove(mt);
    }


}
