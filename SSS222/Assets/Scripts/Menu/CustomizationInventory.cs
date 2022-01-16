using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CustomizationInventory : MonoBehaviour{
    [HeaderAttribute("Objects")]
    [AssetsOnly][SerializeField] GameObject skinElementPrefab;
    [SceneObjectsOnly][SerializeField] Image shipUI;
    [DisableInEditorMode][SceneObjectsOnly][SerializeField] Image shipUI_Overlay;
    [SceneObjectsOnly][SerializeField] RectTransform listContent;
    [SceneObjectsOnly][SerializeField] GameObject sliders;
    [SceneObjectsOnly][SerializeField] Slider Hslider;
    [SceneObjectsOnly][SerializeField] Slider Sslider;
    [SceneObjectsOnly][SerializeField] Slider Vslider;
    [SceneObjectsOnly][SerializeField] Image SsliderIMG;
    [SceneObjectsOnly][SerializeField] Image VsliderIMG;
    [AssetsOnly][SerializeField] Material gradientShader;
    [HeaderAttribute("Rarity Colors")]    
    public Color defColor=Color.grey;
    public Color commonColor=Color.green;
    public Color rareColor=Color.blue;
    public Color epicColor=Color.magenta;
    public Color legendColor=Color.yellow;
    [HeaderAttribute("Properties")]
    [SerializeField] public SkinCategory categorySelected=SkinCategory.reOne;
    [SerializeField] public string skinName="Mk.22";
    public Color overlayColor=Color.red;
    public float[] overlayColorArr = new float[3]{0,1,1};
    void Start(){
        skinName=SaveSerial.instance.playerData.skinName;
        overlayColorArr[0] = SaveSerial.instance.playerData.overlayColor[0];
        overlayColorArr[1] = SaveSerial.instance.playerData.overlayColor[1];
        overlayColorArr[2] = SaveSerial.instance.playerData.overlayColor[2];
        
        overlayColor = Color.HSVToRGB(overlayColorArr[0], overlayColorArr[1], overlayColorArr[2]);
        if(shipUI_Overlay!=null)shipUI_Overlay.color = overlayColor;
        Hslider.value = overlayColorArr[0];
        Sslider.value = overlayColorArr[1];
        Vslider.value = overlayColorArr[2];
        Hslider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        Sslider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        Vslider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        SsliderIMG.material = Instantiate(gradientShader) as Material;
        VsliderIMG.material = SsliderIMG.material;
        SsliderIMG.material.SetColor("_Color2", Color.HSVToRGB(Hslider.value,1,1));
        VsliderIMG.color = Color.HSVToRGB(Hslider.value, 1, 1);
        CreateAllSkins();
    }
    void OnDestroy(){Destroy(SsliderIMG.material);}
    void OnDisable(){Destroy(SsliderIMG.material);}
    public void ValueChangeCheck(){
        overlayColor = Color.HSVToRGB(Hslider.value, Sslider.value, Vslider.value);
        if(shipUI_Overlay!=null)shipUI_Overlay.color = overlayColor;
        SsliderIMG.material.SetColor("_Color2", Color.HSVToRGB(Hslider.value,1,1));
        VsliderIMG.color = Color.HSVToRGB(Hslider.value, 1, 1);
        Color.RGBToHSV(overlayColor, out overlayColorArr[0], out overlayColorArr[1], out overlayColorArr[2]);
    }
    void Update(){
        if(shipUI.transform.childCount>1){shipUI_Overlay=shipUI.transform.GetChild(1).GetComponent<Image>();}
        if(GetSkin(skinName).sprOverlay!=null){
            foreach(Transform go in sliders.transform){go.gameObject.SetActive(true);}
            if(shipUI_Overlay!=null)shipUI_Overlay.gameObject.SetActive(true);
            //SetSkin(skinName);
            SetSkinOverlay(skinName);
            if(shipUI_Overlay!=null)shipUI_Overlay.color=overlayColor;
        }
        else{
            //SetSkin(skinName);
            foreach(Transform go in sliders.transform){if(go.gameObject.activeSelf==true)go.gameObject.SetActive(false);}
            if(shipUI_Overlay!=null)shipUI_Overlay.gameObject.SetActive(false);
        }
    }

    void CreateAllSkins(){
        var currentCategorySkins=Array.FindAll(GameAssets.instance.skins,x=>x.category==categorySelected);
        foreach(GSkin gs in currentCategorySkins){
            var go=Instantiate(skinElementPrefab,listContent);
            go.name="SkinElement_"+gs.name;
            go.GetComponent<Button>().onClick.AddListener(delegate{SetSkin(gs.name);});
            go.GetComponent<Image>().color=GetRarityColor(gs.rarity);
            go.transform.GetChild(0).GetComponent<Image>().sprite=gs.spr;
        }
    }
    //void ChangeCategory(){CreateAllSkins;}

    Color GetRarityColor(SkinRarity rarity){
        var col=Color.white;
        if(rarity==SkinRarity.def){col=defColor;}
        else if(rarity==SkinRarity.common){col=commonColor;}
        else if(rarity==SkinRarity.rare){col=rareColor;}
        else if(rarity==SkinRarity.epic){col=epicColor;}
        else if(rarity==SkinRarity.legend){col=legendColor;}
        return col;
    }
    GSkin GetSkin(string str){return GameAssets.instance.GetSkin(str);}
    int GetSkinID(string str){return GameAssets.instance.GetSkinID(str);}
    GSkin GetSkinByID(int i){return GameAssets.instance.GetSkinByID(i);}
    //public void SetSkinCurrent(){shipUI.sprite=GetSkin(skinName).spr;}
    public void SetSkin(string str){if(GetSkin(str)!=null){skinName=str;shipUI.sprite=GetSkin(str).spr;}}
    public void SetSkinOverlay(string str){if(GetSkin(skinName)!=null){if(GetSkin(skinName).sprOverlay!=null){shipUI_Overlay.sprite=GetSkin(skinName).sprOverlay;}}}
}
