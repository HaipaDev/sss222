using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CustomizationInventory : MonoBehaviour{
    [HeaderAttribute("Objects")]
    [SceneObjectsOnly][SerializeField] Image skin;
    [SceneObjectsOnly][SerializeField] Image skinOverlay;
    [SceneObjectsOnly][SerializeField] GameObject sliders;
    [SceneObjectsOnly][SerializeField] Slider Hslider;
    [SceneObjectsOnly][SerializeField] Slider Sslider;
    [SceneObjectsOnly][SerializeField] Slider Vslider;
    [SceneObjectsOnly][SerializeField] Image SsliderIMG;
    [SceneObjectsOnly][SerializeField] Image VsliderIMG;
    [AssetsOnly][SerializeField] Material gradientShader;
    [HeaderAttribute("Properties")]
    [SerializeField] public string skinName="Mk.22";
    public Color chameleonColor=Color.white;
    public float[] chameleonColorArr = new float[3];
    void Start(){
        //if(skinID<1){skinID=1;}
        skinName=SaveSerial.instance.playerData.skinName;
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
        if(GetSkin(skinName).sprOverlay!=null){
            foreach(Transform go in sliders.transform){go.gameObject.SetActive(true);}
            skinOverlay.gameObject.SetActive(true);
            SetSkin(skinName);
            SetSkinOverlay(skinName);
            skinOverlay.color=chameleonColor;
        }
        else{
            SetSkin(skinName);
            foreach(Transform go in sliders.transform){if(go.gameObject.activeSelf==true)go.gameObject.SetActive(false);}
            skinOverlay.gameObject.SetActive(false);
        }
    }
    GSkin GetSkin(string str){return GameAssets.instance.GetSkin(str);}
    int GetSkinID(string str){return GameAssets.instance.GetSkinID(str);}
    GSkin GetSkinByID(int i){return GameAssets.instance.GetSkinByID(i);}
    public void SetSkin(string str){if(GetSkin(skinName)!=null){skinName=str;skin.sprite=GetSkin(skinName).spr;}}
    public void SetSkinOverlay(string str){if(GetSkin(skinName)!=null){if(GetSkin(skinName).sprOverlay!=null){skinOverlay.sprite=GetSkin(skinName).sprOverlay;}}}
    public void NextSkin(){
        if(GetSkinID(skinName)<GameAssets.instance.skins.Length-1){SetSkin(GetSkinByID(GetSkinID(skinName)+1).name);return;}
        if(GetSkinID(skinName)==GameAssets.instance.skins.Length-1)SetSkin(GetSkinByID(0).name);
    }public void PrevSkin(){
        if(GetSkinID(skinName)>0){SetSkin(GetSkinByID(GetSkinID(skinName)-1).name);return;}
        if(GetSkinID(skinName)==0)SetSkin(GetSkinByID(GameAssets.instance.skins.Length-1).name);
    }
}
