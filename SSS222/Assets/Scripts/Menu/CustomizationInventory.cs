using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CustomizationInventory : MonoBehaviour{
    public static CustomizationInventory instance;
    [HeaderAttribute("Objects")]
    [AssetsOnly][SerializeField] GameObject skinElementPrefab;
    [SceneObjectsOnly][SerializeField] RectTransform skinsListContent;
    [SceneObjectsOnly][SerializeField] RectTransform variantsPanel;
    [SceneObjectsOnly][SerializeField] RectTransform variantsListContent;
    [SceneObjectsOnly][SerializeField] GameObject colorSliders;
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
    [SerializeField] public CstmzCategory categorySelected=CstmzCategory.twoPiece;
    [SerializeField] public string skinName="Mk.22";
    public Color overlayColor=Color.red;
    public float[] overlayColorArr = new float[3]{0,1,1};
    bool loaded;
    void Awake(){instance=this;}
    void Start(){
        skinName=SaveSerial.instance.playerData.skinName;
        categorySelected=Array.Find(GameAssets.instance.skins,x=>x.name.Contains(GetSkinName(SaveSerial.instance.playerData.skinName))).category;

        overlayColorArr[0] = SaveSerial.instance.playerData.overlayColor[0];
        overlayColorArr[1] = SaveSerial.instance.playerData.overlayColor[1];
        overlayColorArr[2] = SaveSerial.instance.playerData.overlayColor[2];
        loaded=true;
        
        overlayColor = Color.HSVToRGB(overlayColorArr[0], overlayColorArr[1], overlayColorArr[2]);
        Hslider.value = overlayColorArr[0];
        Sslider.value = overlayColorArr[1];
        Vslider.value = overlayColorArr[2];

        SsliderIMG.material = Instantiate(gradientShader) as Material;
        VsliderIMG.material = SsliderIMG.material;
        SsliderIMG.material.SetColor("_Color2", Color.HSVToRGB(Hslider.value,1,1));
        VsliderIMG.color = Color.HSVToRGB(Hslider.value, 1, 1);
        DeleteAllSkinElements();CreateAllSkinElements();
        HighlightSelectedSkin();
    }
    //void OnDestroy(){DestroyImmediate(SsliderIMG.material);}
    //void OnDisable(){DestroyImmediate(SsliderIMG.material);}
    public void ValueChangeCheck(){
        overlayColorArr[0]=Hslider.value;
        overlayColorArr[1]=Sslider.value;
        overlayColorArr[2]=Vslider.value;
        overlayColor=Color.HSVToRGB(overlayColorArr[0],overlayColorArr[1],overlayColorArr[2]);

        SsliderIMG.material.SetColor("_Color2",Color.HSVToRGB(Hslider.value,1,1));
        VsliderIMG.color = Color.HSVToRGB(Hslider.value, 1, 1);
    }
    void Update(){
        if(loaded){
            SaveSerial.instance.playerData.skinName=skinName;
            SaveSerial.instance.playerData.overlayColor[0]=overlayColorArr[0];
            SaveSerial.instance.playerData.overlayColor[1]=overlayColorArr[1];
            SaveSerial.instance.playerData.overlayColor[2]=overlayColorArr[2];
        }

        ShipCustomizationManager.instance.skinName=skinName;
        ShipCustomizationManager.instance.overlayColor=Color.HSVToRGB(overlayColorArr[0],overlayColorArr[1],overlayColorArr[2]);
    }

    void DeleteAllSkinElements(){foreach(Transform t in skinsListContent){Destroy(t.gameObject);}}
    void CreateAllSkinElements(){
        var currentCategorySkins=Array.FindAll(GameAssets.instance.skins,x=>x.category==categorySelected);
        foreach(CstmzSkin gs in currentCategorySkins){
            var go=Instantiate(skinElementPrefab,skinsListContent);
            go.name="SkinElement_"+gs.name;
            go.GetComponent<SkinElement>().skinName=gs.name;
            go.GetComponent<SkinElement>().rarity=gs.rarity;
            go.transform.GetChild(1).GetComponent<Image>().sprite=gs.spr;
            go.transform.GetChild(2).GetComponent<Image>().sprite=gs.sprOverlay;
        }
    }
    void HighlightSelectedSkin(){foreach(Transform t in skinsListContent){
        if(GetSkinName(skinName)==t.GetComponent<SkinElement>().skinName){
            t.GetChild(0).gameObject.SetActive(true);
            if(skinName.Contains("_"))t.GetChild(1).GetComponent<Image>().sprite=GetSkinSprite(skinName);
        }
        else{t.GetChild(0).gameObject.SetActive(false);}
    }}
    
    [HideInInspector]public string[] _CstmzCategoryNames=new string[]{"Special","Shop","ReOne","TwoPiece"};
    public void ChangeCategory(string str){
        if(str.Contains(_CstmzCategoryNames[0])){categorySelected=CstmzCategory.special;}
        else if(str.Contains(_CstmzCategoryNames[1])){categorySelected=CstmzCategory.shop;}
        else if(str.Contains(_CstmzCategoryNames[2])){categorySelected=CstmzCategory.reOne;}
        else if(str.Contains(_CstmzCategoryNames[3])){categorySelected=CstmzCategory.twoPiece;}
        DeleteAllSkinElements();CreateAllSkinElements();
        HighlightSelectedSkin();
    }


    public void OpenVariants(string str){
        if(GetSkin(str).variants.Length>0||GetOverlaySprite(str)!=null){
            variantsPanel.gameObject.SetActive(true);
            if(GetOverlaySprite(str)!=null||Array.Find(GetSkin(str).variants,x=>x.sprOverlay!=null)!=null){colorSliders.SetActive(true);}
            DeleteAllVariantElements();
            CreateAllVariantElements(str);
            HighlightSelectedVariant();
        }
    }
    void DeleteAllVariantElements(){foreach(Transform t in variantsListContent){Destroy(t.gameObject);}}
    void CreateAllVariantElements(string str){
        //Create first default variant
        var go1=Instantiate(skinElementPrefab,variantsListContent);
        go1.name="SkinVariant_-1";
        go1.GetComponent<SkinElement>().variant=true;
        go1.GetComponent<SkinElement>().variantId=-1;
        go1.GetComponent<SkinElement>().skinName=GetSkin(str).name;
        go1.GetComponent<SkinElement>().rarity=GetSkin(str).rarity;
        go1.transform.GetChild(1).GetComponent<Image>().sprite=GetSkinSprite(str);
        if(GetOverlaySprite(str)!=null)go1.transform.GetChild(2).GetComponent<Image>().sprite=GetOverlaySprite(str);
        //Create all others
        CstmzSkinVariant[] variants=GetSkin(str).variants;
        for(int i=0;i<variants.Length;i++){
            var gs=variants[i];
            var go=Instantiate(skinElementPrefab,variantsListContent);
            go.name="SkinVariant_"+i;
            go.GetComponent<SkinElement>().variant=true;
            go.GetComponent<SkinElement>().variantId=i;
            go.GetComponent<SkinElement>().skinName=GetSkin(str).name;
            go.GetComponent<SkinElement>().rarity=GetSkin(str).rarity;
            go.transform.GetChild(1).GetComponent<Image>().sprite=gs.spr;
            //if(GetSkinSprite(str)!=null){go.transform.GetChild(2).GetComponent<Image>().sprite=GetSkinSprite(str);}
            if(gs.sprOverlay!=null){go.transform.GetChild(2).GetComponent<Image>().sprite=gs.sprOverlay;}
        }
    }
    void HighlightSelectedVariant(){foreach(Transform t in variantsListContent){
        if((skinName.Contains("_")&&int.Parse(skinName.Split('_')[1])==t.GetComponent<SkinElement>().variantId)||
        (!skinName.Contains("_")&&GetSkinName(skinName)==t.GetComponent<SkinElement>().skinName&&t.GetComponent<SkinElement>().variantId==-1)){t.GetChild(0).gameObject.SetActive(true);}
        else{t.GetChild(0).gameObject.SetActive(false);}
    }}

    public Color GetRarityColor(CstmzRarity rarity){
        var col=Color.white;
        switch(rarity){
            case CstmzRarity.def:col=defColor;break;
            case CstmzRarity.common:col=commonColor;break;
            case CstmzRarity.rare:col=rareColor;break;
            case CstmzRarity.epic:col=epicColor;break;
            case CstmzRarity.legend:col=legendColor;break;
        }
        return col;
    }
    string GetSkinName(string str){string _str=str;if(skinName.Contains("_")){_str=skinName.Split('_')[0];}return _str;}
    public CstmzSkin GetSkin(string str){string _str=str;if(_str.Contains("_")){_str=_str.Split('_')[0];}return GameAssets.instance.GetSkin(_str);}
    CstmzSkin GetSkinCurrent(){return GetSkin(skinName);}
    int GetSkinID(string str){return GameAssets.instance.GetSkinID(str);}
    CstmzSkin GetSkinByID(int i){return GameAssets.instance.GetSkinByID(i);}
    Sprite GetSkinSprite(string str){return ShipCustomizationManager.instance.GetSkinSprite(str);}
    Sprite GetOverlaySprite(string str){return ShipCustomizationManager.instance.GetOverlaySprite(str);}
    CstmzSkinVariant GetSkinVariant(string str,int id){return GameAssets.instance.GetSkinVariant(str,id);}
    public void SetSkin(string str){skinName=str;if(variantsPanel.gameObject.activeSelf){variantsPanel.gameObject.SetActive(false);colorSliders.SetActive(false);}HighlightSelectedSkin();}
}