using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AyModulu : ModulTabani
{
    [SerializeField]
    private Light ay;//Ay ışığı için değişken oluşturuldu.
    [SerializeField] //İşaretçi adrese erişmek için yine devrede
    private Gradient ayRengi;
    [SerializeField]
    private float tabanYogunlugu;
    public override void ModulGuncelle(float yogunluk)
    {
        ay.color = ayRengi.Evaluate(1 - yogunluk);//Ay rengi değişimi belirlendi.
        ay.intensity = (1 - yogunluk) * tabanYogunlugu + 0.05f; //Ay ışığı yoğunluğu 
    }

}
