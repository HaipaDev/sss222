using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour{
    [SerializeField] public int skinID=0;
    [SerializeField] Sprite[] skins;
    [SerializeField] Sprite[] overlays;
    [HeaderAttribute("Objects")]
    [SerializeField] Image skin;
    [SerializeField] Image skinOverlay;

    [SerializeField] GameObject sliders;
    [SerializeField] Slider Hslider;
    [SerializeField] Slider Sslider;
    [SerializeField] Slider Vslider;
    [SerializeField] Image SsliderIMG;
    [SerializeField] Image VsliderIMG;
    [SerializeField] Material gradientShader;
    [HeaderAttribute("Properties")]
    public int chameleonOverlayID;
    public Color chameleonColor;
    public float[] chameleonColorArr = new float[3];
    void Start(){
        //if(skinID<1){skinID=1;}
        skins=FindObjectOfType<GameAssets>().skins;
        overlays=FindObjectOfType<GameAssets>().skinOverlays;
        skinID=SaveSerial.instance.playerData.skinID;
        chameleonColorArr[0] = SaveSerial.instance.playerData.chameleonColor[0];
        chameleonColorArr[1] = SaveSerial.instance.playerData.chameleonColor[1];
        chameleonColorArr[2] = SaveSerial.instance.playerData.chameleonColor[2];
        
        chameleonColor = Color.HSVToRGB(chameleonColorArr[0], chameleonColorArr[1], chameleonColorArr[2]);
        skinOverlay.color = chameleonColor;
        //float H;float S=1; float V=1;
        //Color.RGBToHSV(chameleonColor,out H,out S,out V);
        Hslider.value = chameleonColorArr[0];
        Sslider.value = chameleonColorArr[1];
        Vslider.value = chameleonColorArr[2];
        Hslider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        Sslider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        Vslider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        SsliderIMG.material = Instantiate(gradientShader) as Material;
        VsliderIMG.material = SsliderIMG.material;
        SsliderIMG.material.SetColor("_Color2", Color.HSVToRGB(Hslider.value,1,1));
        VsliderIMG.color = Color.HSVToRGB(Hslider.value, 1, 1);
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck(){
        chameleonColor = Color.HSVToRGB(Hslider.value, Sslider.value, Vslider.value);
        skinOverlay.color = chameleonColor;
        SsliderIMG.material.SetColor("_Color2", Color.HSVToRGB(Hslider.value,1,1));
        VsliderIMG.color = Color.HSVToRGB(Hslider.value, 1, 1);
        Color.RGBToHSV(chameleonColor, out chameleonColorArr[0], out chameleonColorArr[1], out chameleonColorArr[2]);
    }
    void Update() {
        if (skinID==1){
            foreach(Transform go in sliders.transform){go.gameObject.SetActive(true);}
            skinOverlay.gameObject.SetActive(true);
            skin.sprite=skins[skinID];
            skinOverlay.sprite = overlays[chameleonOverlayID];
            //chameleonOvColor.a = chameleonOvAlpha;
            skinOverlay.color = chameleonColor;
        }
        else{
            skin.sprite=skins[skinID];
            foreach(Transform go in sliders.transform){if(go.gameObject.activeSelf==true)go.gameObject.SetActive(false);}
            skinOverlay.gameObject.SetActive(false);
        }
    }
    public void ChangeSkin(int ID){
        skinID=ID;
    }public void NextSkin(){
        if(skinID<skins.Length-1){skinID++;return;}
        if(skinID==skins.Length-1)skinID=0;
    }public void PrevSkin(){
        if(skinID>0){skinID--;return;}
        if(skinID==0)skinID=skins.Length-1;
    }
}
