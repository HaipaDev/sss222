using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CustomizationInventory : MonoBehaviour{
    public static CustomizationInventory instance;
    [HeaderAttribute("Objects")]
    [SceneObjectsOnly][SerializeField] RectTransform typesListConent;
    [AssetsOnly][SerializeField] GameObject cstmzElementPrefab;
    [SceneObjectsOnly][SerializeField] RectTransform elementsListContent;
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
    [SerializeField] public CstmzType typeSelected=CstmzType.skin;
    [SerializeField] public string skinName="Mk.22";
    public Color overlayColor=Color.red;
    public float[] overlayColorArr = new float[3]{0,1,1};
    [SerializeField] public string trailName="Flame";
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

        trailName=SaveSerial.instance.playerData.trailName;

        HighlightSelectedType();
        //RecreateAllElements();
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
            SaveSerial.instance.playerData.trailName=trailName;
        }

        ShipCustomizationManager.instance.skinName=skinName;
        ShipCustomizationManager.instance.overlayColor=Color.HSVToRGB(overlayColorArr[0],overlayColorArr[1],overlayColorArr[2]);
        ShipCustomizationManager.instance.trailName=trailName;
    }

    public void RecreateAllElements(){DeleteAllElements();CreateAllElements();HighlightSelectedElement();}
    //public void RecreateAllElements(){StartCoroutine(RecreateAllElementsI());}
    public IEnumerator RecreateAllElementsI(){DeleteAllElements();
        yield return new WaitForSeconds(0.01f);CreateAllElements();
        yield return new WaitForSeconds(0.01f);HighlightSelectedElement();}
    void DeleteAllElements(){foreach(Transform t in elementsListContent){Destroy(t.gameObject);}}
    void CreateAllElements(){
        if(typeSelected==CstmzType.skin){
            var currentCategorySkins=Array.FindAll(GameAssets.instance.skins,x=>x.category==categorySelected);
            foreach(CstmzSkin ge in currentCategorySkins){
                var go=Instantiate(cstmzElementPrefab,elementsListContent);
                go.name="SkinElement_"+ge.name;
                CstmzElement ce=go.GetComponent<CstmzElement>();
                ce.elementType=typeSelected;
                ce.elementName=ge.name;
                ce.rarity=ge.rarity;
                go.transform.GetChild(1).GetComponent<Image>().sprite=ge.spr;
                go.transform.GetChild(2).GetComponent<Image>().sprite=ge.sprOverlay;
            }
        }else if(typeSelected==CstmzType.trail){
            var currentCategoryTrails=Array.FindAll(GameAssets.instance.trails,x=>x.category==categorySelected);
            foreach(CstmzTrail ge in currentCategoryTrails){
                var go=Instantiate(cstmzElementPrefab,elementsListContent);
                go.name="TrailElement_"+ge.name;
                CstmzElement ce=go.GetComponent<CstmzElement>();
                ce.elementType=typeSelected;
                ce.elementName=ge.name;
                ce.rarity=ge.rarity;
                Destroy(go.transform.GetChild(2).gameObject);
                Destroy(go.transform.GetChild(1).gameObject);ce.elementPv=Instantiate(ge.part,go.transform);
                GameAssets.instance.RegularParticleIntoUIParticle(ce.elementPv);
            }
        }
    }
    void HighlightSelectedElement(){foreach(Transform t in elementsListContent){
        CstmzElement ce=t.GetComponent<CstmzElement>();
        if(ce.elementType==CstmzType.skin&&t.GetComponent<CstmzElement>().elementName==GetSkinName(skinName)){
            ce.selectedBg.SetActive(true);
            if(skinName.Contains("_"))t.GetChild(1).GetComponent<Image>().sprite=GetSkinSprite(skinName);
        }else{ce.selectedBg.SetActive(false);}
        
        if(ce.elementType==CstmzType.trail&&t.GetComponent<CstmzElement>().elementName==trailName){
            ce.selectedBg.SetActive(true);
        }else{ce.selectedBg.SetActive(false);}
    }}
    void HighlightSelectedType(){foreach(Transform t in typesListConent){
        CstmzTypeElement ce=t.GetComponent<CstmzTypeElement>();
        if(ce.elementType==typeSelected){
            ce.selectedBg.SetActive(true);
            //if(skinName.Contains("_"))t.GetChild(1).GetComponent<Image>().sprite=GetSkinSprite(skinName);
        }
        else{ce.selectedBg.SetActive(false);}

        if(ce.elementType==CstmzType.skin){
            t.GetChild(1).GetComponent<Image>().sprite=GetSkinSprite(skinName);
        }else if(ce.elementType==CstmzType.trail){
            Destroy(ce.overlayImg);
            Destroy(ce.elementPv);ce.elementPv=Instantiate(GetTrail(trailName).part,t);
            GameAssets.instance.RegularParticleIntoUIParticle(ce.elementPv);
        }
    }}
    
    [HideInInspector]public string[] _CstmzCategoryNames=new string[]{"Special","Shop","ReOne","TwoPiece"};
    public void ChangeCategory(string str){
        if(str.Contains(_CstmzCategoryNames[0])){categorySelected=CstmzCategory.special;}
        else if(str.Contains(_CstmzCategoryNames[1])){categorySelected=CstmzCategory.shop;}
        else if(str.Contains(_CstmzCategoryNames[2])){categorySelected=CstmzCategory.reOne;}
        else if(str.Contains(_CstmzCategoryNames[3])){categorySelected=CstmzCategory.twoPiece;}
        RecreateAllElements();
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
        var go1=Instantiate(cstmzElementPrefab,variantsListContent);
        go1.name="SkinVariant_-1";
        CstmzElement ce=go1.GetComponent<CstmzElement>();
        ce.variant=true;
        ce.variantId=-1;
        ce.elementName=GetSkin(str).name;
        ce.rarity=GetSkin(str).rarity;
        ce.elementPv.GetComponent<Image>().sprite=GetSkinSprite(str);
        if(GetOverlaySprite(str)!=null)ce.overlayImg.GetComponent<Image>().sprite=GetOverlaySprite(str);
        //Create all others
        CstmzSkinVariant[] variants=GetSkin(str).variants;
        for(int i=0;i<variants.Length;i++){
            var gs=variants[i];
            var go=Instantiate(cstmzElementPrefab,variantsListContent);
            go.name="SkinVariant_"+i;
            ce.variant=true;
            ce.variantId=i;
            ce.elementName=GetSkin(str).name;
            ce.rarity=GetSkin(str).rarity;
            go.transform.GetChild(1).GetComponent<Image>().sprite=gs.spr;
            //if(GetSkinSprite(str)!=null){go.transform.GetChild(2).GetComponent<Image>().sprite=GetSkinSprite(str);}
            if(gs.sprOverlay!=null){go.transform.GetChild(2).GetComponent<Image>().sprite=gs.sprOverlay;}
        }
    }
    void HighlightSelectedVariant(){foreach(Transform t in variantsListContent){
        CstmzElement ce=t.GetComponent<CstmzElement>();
        if((skinName.Contains("_")&&int.Parse(skinName.Split('_')[1])==ce.variantId)||
        (!skinName.Contains("_")&&GetSkinName(skinName)==ce.elementName&&ce.variantId==-1)){ce.selectedBg.SetActive(true);}
        else{ce.selectedBg.SetActive(false);}
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
    public void SetType(CstmzType type){if(typeSelected!=type){typeSelected=type;HighlightSelectedType();RecreateAllElements();}}

    string GetSkinName(string str){string _str=str;if(skinName.Contains("_")){_str=skinName.Split('_')[0];}return _str;}
    public CstmzSkin GetSkin(string str){string _str=str;if(_str.Contains("_")){_str=_str.Split('_')[0];}return GameAssets.instance.GetSkin(_str);}
    CstmzSkin GetSkinCurrent(){return GetSkin(skinName);}
    int GetSkinID(string str){return GameAssets.instance.GetSkinID(str);}
    CstmzSkin GetSkinByID(int i){return GameAssets.instance.GetSkinByID(i);}
    Sprite GetSkinSprite(string str){return ShipCustomizationManager.instance.GetSkinSprite(str);}
    Sprite GetOverlaySprite(string str){return ShipCustomizationManager.instance.GetOverlaySprite(str);}
    CstmzSkinVariant GetSkinVariant(string str,int id){return GameAssets.instance.GetSkinVariant(str,id);}
    public void SetSkin(string str){skinName=str;if(variantsPanel.gameObject.activeSelf){variantsPanel.gameObject.SetActive(false);colorSliders.SetActive(false);}HighlightSelectedElement();HighlightSelectedType();}

    public CstmzTrail GetTrail(string str){return GameAssets.instance.GetTrail(str);}
    public void SetTrail(string str){trailName=str;HighlightSelectedElement();HighlightSelectedType();}
}