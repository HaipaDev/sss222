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
    [SerializeField] public string flaresName="Flares";
    [SerializeField] public string deathFxName="Explosion";
    [SerializeField] public string musicName="Find You";
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
        flaresName=SaveSerial.instance.playerData.flaresName;
        deathFxName=SaveSerial.instance.playerData.deathFxName;
        musicName=SaveSerial.instance.playerData.musicName;

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
            SaveSerial.instance.playerData.flaresName=flaresName;
            SaveSerial.instance.playerData.deathFxName=deathFxName;
            SaveSerial.instance.playerData.musicName=musicName;
        }

        ShipCustomizationManager.instance.skinName=skinName;
        ShipCustomizationManager.instance.overlayColor=Color.HSVToRGB(overlayColorArr[0],overlayColorArr[1],overlayColorArr[2]);
        ShipCustomizationManager.instance.trailName=trailName;
        ShipCustomizationManager.instance.flaresName=flaresName;
        ShipCustomizationManager.instance.deathFxName=deathFxName;
        ShipCustomizationManager.instance.musicName=musicName;

        RefreshParticles();
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
                ce.elementPv.GetComponent<Image>().sprite=ge.spr;
                ce.overlayImg.GetComponent<Image>().sprite=ge.sprOverlay;
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
                Destroy(ce.overlayImg);
                Destroy(ce.elementPv);ce.elementPv=Instantiate(ge.part,go.transform);
                GameAssets.instance.TransformIntoUIParticle(ce.elementPv);
            }
        }else if(typeSelected==CstmzType.flares){
            var currentCategoryFlares=Array.FindAll(GameAssets.instance.flares,x=>x.category==categorySelected);
            foreach(CstmzFlares ge in currentCategoryFlares){
                var go=Instantiate(cstmzElementPrefab,elementsListContent);
                go.name="FlaresElement_"+ge.name;
                CstmzElement ce=go.GetComponent<CstmzElement>();
                ce.elementType=typeSelected;
                ce.elementName=ge.name;
                ce.rarity=ge.rarity;
                Destroy(ce.overlayImg);
                Destroy(ce.elementPv.GetComponent<Image>());
                for(var i=0;i<ce.elementPv.transform.childCount;i++){Destroy(ce.elementPv.transform.GetChild(i).gameObject);}
                GameObject goPt=Instantiate(GetFlareVFX(ge.name),ce.elementPv.transform);goPt.transform.localPosition=new Vector2(-44,0);
                GameAssets.instance.TransformIntoUIParticle(goPt,1,-1);
                goPt=Instantiate(GetFlareVFX(ge.name),ce.elementPv.transform);goPt.transform.localPosition=new Vector2(44,0);
                GameAssets.instance.TransformIntoUIParticle(goPt,1,-1);
            }
        }else if(typeSelected==CstmzType.deathFx){
            var currentCategoryDeathFxs=Array.FindAll(GameAssets.instance.deathFxs,x=>x.category==categorySelected);
            foreach(CstmzDeathFx ge in currentCategoryDeathFxs){
                var go=Instantiate(cstmzElementPrefab,elementsListContent);
                go.name="DeathFxElement_"+ge.name;
                CstmzElement ce=go.GetComponent<CstmzElement>();
                ce.elementType=typeSelected;
                ce.elementName=ge.name;
                ce.rarity=ge.rarity;
                Destroy(ce.overlayImg);
                Destroy(ce.elementPv);
                GameObject goPt=Instantiate(ge.obj,go.transform);
                GameAssets.instance.TransformIntoUIParticle(goPt,1,-1);
            }
        }else if(typeSelected==CstmzType.music){
            var currentCategoryMusic=Array.FindAll(GameAssets.instance.musics,x=>x.category==categorySelected);
            foreach(CstmzMusic ge in currentCategoryMusic){
                var go=Instantiate(cstmzElementPrefab,elementsListContent);
                go.name="MusicElement_"+ge.name;
                CstmzElement ce=go.GetComponent<CstmzElement>();
                ce.elementType=typeSelected;
                ce.elementName=ge.name;
                ce.rarity=ge.rarity;
                ce.elementPv.GetComponent<Image>().sprite=ge.icon;
                Destroy(ce.overlayImg);
            }
        }
    }
    void HighlightSelectedElement(){foreach(Transform t in elementsListContent){
        CstmzElement ce=t.GetComponent<CstmzElement>();
        if(typeSelected==CstmzType.skin&&ce.elementName==GetSkinName(skinName)){
            ce.selectedBg.SetActive(true);
            if(skinName.Contains("_"))ce.elementPv.GetComponent<Image>().sprite=GetSkinSprite(skinName);
        }else{ce.selectedBg.SetActive(false);}
        if(typeSelected==CstmzType.trail&&ce.elementName==trailName){
            ce.selectedBg.SetActive(true);
        }else{ce.selectedBg.SetActive(false);}
        if(typeSelected==CstmzType.flares&&ce.elementName==flaresName){
            ce.selectedBg.SetActive(true);
        }else{ce.selectedBg.SetActive(false);}
        if(typeSelected==CstmzType.deathFx&&ce.elementName==deathFxName){
            ce.selectedBg.SetActive(true);
        }else{ce.selectedBg.SetActive(false);}
        if(typeSelected==CstmzType.music&&ce.elementName==musicName){
            ce.selectedBg.SetActive(true);
        }else{ce.selectedBg.SetActive(false);}
    }}
    void HighlightSelectedType(){foreach(Transform t in typesListConent){
        CstmzTypeElement ce=t.GetComponent<CstmzTypeElement>();
        if(ce.elementType==typeSelected){
            ce.selectedBg.SetActive(true);
            //if(skinName.Contains("_"))ce.elementPv.GetComponent<Image>().sprite=GetSkinSprite(skinName);
        }
        else{ce.selectedBg.SetActive(false);}

        if(ce.elementType==CstmzType.skin){
            ce.elementPv.GetComponent<Image>().sprite=GetSkinSprite(skinName);
        }else if(ce.elementType==CstmzType.trail){
            Destroy(ce.overlayImg);
            Destroy(ce.elementPv);ce.elementPv=Instantiate(GetTrail(trailName).part,t);
            GameAssets.instance.TransformIntoUIParticle(ce.elementPv);
        }else if(ce.elementType==CstmzType.flares){
            Destroy(ce.overlayImg);
            for(var i=0;i<ce.elementPv.transform.childCount;i++){Destroy(ce.elementPv.transform.GetChild(i).gameObject);}
            GameObject goPt=Instantiate(GetFlareVFX(ce.elementName),ce.elementPv.transform);goPt.transform.localPosition=new Vector2(-44,0);
            GameAssets.instance.TransformIntoUIParticle(goPt,1,-1);
            goPt=Instantiate(GetFlareVFX(ce.elementName),ce.elementPv.transform);goPt.transform.localPosition=new Vector2(44,0);
            GameAssets.instance.TransformIntoUIParticle(goPt,1,-1);
        }else if(ce.elementType==CstmzType.deathFx){
            Destroy(ce.overlayImg);
            Destroy(ce.elementPv);
            ce.elementPv=Instantiate(GetDeathFxObj(ce.elementName),ce.transform);
            GameAssets.instance.TransformIntoUIParticle(ce.elementPv,1,-1);
        }if(ce.elementType==CstmzType.music){
            ce.elementPv.GetComponent<Image>().sprite=GetMusic(musicName).icon;
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
            ce.elementPv.GetComponent<Image>().sprite=gs.spr;
            //if(GetSkinSprite(str)!=null){ce.overlayImg.GetComponent<Image>().sprite=GetSkinSprite(str);}
            if(gs.sprOverlay!=null){ce.overlayImg.GetComponent<Image>().sprite=gs.sprOverlay;}
        }
    }
    void HighlightSelectedVariant(){foreach(Transform t in variantsListContent){
        CstmzElement ce=t.GetComponent<CstmzElement>();
        if((skinName.Contains("_")&&int.Parse(skinName.Split('_')[1])==ce.variantId)||
        (!skinName.Contains("_")&&GetSkinName(skinName)==ce.elementName&&ce.variantId==-1)){ce.selectedBg.SetActive(true);}
        else{ce.selectedBg.SetActive(false);}
    }}

    void RefreshParticles(){
        if(typeSelected==CstmzType.flares){
            foreach(CstmzElement ce in elementsListContent.GetComponentsInChildren<CstmzElement>()){
                if(ce.elementPv.transform.childCount==0){
                    GameObject goPt=Instantiate(GetFlareVFX(ce.elementName),ce.elementPv.transform);goPt.transform.localPosition=new Vector2(-44,0);
                    GameAssets.instance.TransformIntoUIParticle(goPt,1,-1);
                    goPt=Instantiate(GetFlareVFX(ce.elementName),ce.elementPv.transform);goPt.transform.localPosition=new Vector2(44,0);
                    GameAssets.instance.TransformIntoUIParticle(goPt,1,-1);
                }
            }
        }
        CstmzTypeElement typeFlares=Array.Find(typesListConent.GetComponentsInChildren<CstmzTypeElement>(),x=>x.elementType==CstmzType.flares);
        if(typeFlares.elementPv.transform.childCount==0){
            GameObject goPt=Instantiate(GetFlareVFX(flaresName),typeFlares.elementPv.transform);goPt.transform.localPosition=new Vector2(-44,0);
            GameAssets.instance.TransformIntoUIParticle(goPt,1,-1);
            goPt=Instantiate(GetFlareVFX(flaresName),typeFlares.elementPv.transform);goPt.transform.localPosition=new Vector2(44,0);
            GameAssets.instance.TransformIntoUIParticle(goPt,1,-1);
        }

        if(typeSelected==CstmzType.deathFx){
            foreach(CstmzElement ce in elementsListContent.GetComponentsInChildren<CstmzElement>()){
                if(ce.elementPv==null){
                    ce.elementPv=Instantiate(GetDeathFxObj(ce.elementName),ce.transform);
                    GameAssets.instance.TransformIntoUIParticle(ce.elementPv,1,-1);
                }
            }
        }
        CstmzTypeElement typeDeathFx=Array.Find(typesListConent.GetComponentsInChildren<CstmzTypeElement>(),x=>x.elementType==CstmzType.deathFx);
        if(typeDeathFx.elementPv==null){
            typeDeathFx.elementPv=Instantiate(GetDeathFxObj(deathFxName),typeDeathFx.transform);
            GameAssets.instance.TransformIntoUIParticle(typeDeathFx.elementPv,1,-1);
        }
    }


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

    public GameObject GetFlareVFX(string str){GameObject go=null;
        if(GameAssets.instance.GetFlares(str)!=null){go=GameAssets.instance.GetFlareRandom(str);}
        return go;
    }
    public void SetFlares(string str){flaresName=str;HighlightSelectedElement();HighlightSelectedType();}

    public CstmzDeathFx GetDeathFx(string str){return GameAssets.instance.GetDeathFx(str);}
    public GameObject GetDeathFxObj(string str){GameObject go=null;
        if(GameAssets.instance.GetDeathFx(str)!=null){go=GameAssets.instance.GetDeathFx(str).obj;}
        return go;
    }
    public void SetDeathFx(string str){deathFxName=str;HighlightSelectedElement();HighlightSelectedType();}
    public CstmzMusic GetMusic(string str){return GameAssets.instance.GetMusic(str);}
    public void SetMusic(string str){musicName=str;HighlightSelectedElement();HighlightSelectedType();}
}