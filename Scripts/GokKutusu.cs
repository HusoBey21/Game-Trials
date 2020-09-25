using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GokKutusu : ModulTabani //Soyut sınıftan türetilmiş alt sınıf
{
    [SerializeField]
  private  Gradient gokRengi;//Gök rengini belirlemek için değişken tanımı yapıldı.
    [SerializeField]
    private Gradient ufukRengi;//Güneş batarken oluşan ufuk çizgisini tanımlar.
    public override void ModulGuncelle(float yogunluk)
    {
        RenderSettings.skybox.SetColor("_SkyTint", gokRengi.Evaluate(yogunluk));
        RenderSettings.skybox.SetColor("_GroundColor", ufukRengi.Evaluate(yogunluk));
    }
   
}
