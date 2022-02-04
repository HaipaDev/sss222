using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ShipCustomizationManager : MonoBehaviour{
    public static ShipCustomizationManager instance;
    [HeaderAttribute("Properties")]
    public string skinName="Mk.22";
    [SceneObjectsOnly][SerializeField] public GameObject overlayObj;
    public Color overlayColor=Color.red;
    public string trailName="Flame";
    [SceneObjectsOnly][SerializeField] public GameObject trailObj;
    Vector2 trailObjPos=Vector2.zero;
    public string flaresName="Flares";
    public string deathFxName="Explosion";
    public string musicName="Find You";

    SpriteRenderer overlaySpr;
    Image overlayImg;
    SpriteRenderer spr;
    Image img;
    void Awake(){instance=this;}
    void Start(){
        spr=GetComponent<SpriteRenderer>();
        if(spr==null)img=GetComponent<Image>();
        LoadValues();
        if(overlayObj!=null){
            overlayObj.transform.position=new Vector3(overlayObj.transform.position.x,overlayObj.transform.position.y,transform.root.position.z-0.01f);
            overlayObj.transform.localScale=Vector3.one;
            overlaySpr=overlayObj.GetComponent<SpriteRenderer>();
            if(overlaySpr==null){overlayImg=overlayObj.GetComponent<Image>();}
            if(GameSession.maskMode!=0&&overlaySpr!=null){overlaySpr.maskInteraction=(SpriteMaskInteraction)GameSession.maskMode;}
        }
    }
    void Update(){
        SetSkin(skinName);
        if(GetOverlaySprite(skinName)!=null)SetOverlay(GetOverlaySprite(skinName),overlayColor);
        else{
            if(overlaySpr!=null)overlaySpr.color=Color.clear;
            if(overlayImg!=null)overlayImg.color=Color.clear;
        }
        SetTrail(trailName);
    }


    //string GetSkinName(){string str=skinName;if(skinName.Contains(" _")){str=skinName.Split('_')[0];}return str;}
    CstmzSkin GetSkin(string str){string _str=str;if(_str.Contains("_")){_str=_str.Split('_')[0];}return GameAssets.instance.GetSkin(_str);}
    CstmzSkin GetSkinCurrent(){string _str=skinName;if(_str.Contains("_")){_str=_str.Split('_')[0];}return GameAssets.instance.GetSkin(_str);}
    CstmzSkinVariant GetSkinVariant(string str,int id){return GameAssets.instance.GetSkinVariant(str,id);}
    public Sprite GetSkinSprite(string str){Sprite spr=null;
        if(str.Contains("_")){spr=GameAssets.instance.GetSkin(str.Split('_')[0]).variants[int.Parse(str.Split('_')[1])].spr;}
        else{spr=GameAssets.instance.GetSkin(str).spr;}
    return spr;}
    public Sprite GetOverlaySprite(string str){Sprite spr=null;
        if(str.Contains("_")){spr=GameAssets.instance.GetSkin(str.Split('_')[0]).variants[int.Parse(str.Split('_')[1])].sprOverlay;}
        else{spr=GameAssets.instance.GetSkin(str).sprOverlay;}
    return spr;}
    void SetSkin(string str){if(spr!=null){if(spr.sprite!=GetSkinSprite(str))spr.sprite=GetSkinSprite(str);}
        else if(img!=null){if(img.sprite!=GetSkinSprite(str))img.sprite=GetSkinSprite(str);}}
    void SetOverlay(Sprite sprite, Color color){
        Color _color=Color.white;if(color!=Color.clear){_color=color;}
        if(overlaySpr!=null){
            if(overlaySpr.sprite!=sprite)overlaySpr.sprite=sprite;
            if(overlaySpr.color!=_color)overlaySpr.color=_color;
        }else if(overlayImg!=null){
            if(overlayImg.sprite!=sprite)overlayImg.sprite=sprite;
            if(overlayImg.color!=_color)overlayImg.color=_color;
        }
    }
    void SetTrail(string str){
        if(GetComponent<TrailVFX>()!=null){if(GameAssets.instance.GetTrail(str)!=null)GetComponent<TrailVFX>().SetNewTrail(str,true);}
        else{if(trailObj!=null){if(trailObjPos==Vector2.zero){trailObjPos=trailObj.transform.localPosition;}
        if(GameAssets.instance.GetTrail(str)!=null){if(!trailObj.name.Contains(GameAssets.instance.GetTrail(str).part.name)){
            var _tempTrailObj=trailObj;trailObj=Instantiate(GameAssets.instance.GetTrail(str).part,transform);trailObj.transform.localPosition=trailObjPos;Destroy(_tempTrailObj);
            GameAssets.instance.TransformIntoUIParticle(trailObj);
        }}}}
    }
    public GameObject GetFlareVFX(){GameObject go=GameAssets.instance.GetVFX("FlareShoot");if(GameAssets.instance.GetFlares(flaresName)!=null){go=GameAssets.instance.GetFlareRandom(flaresName);}return go;}
    public CstmzDeathFx GetDeathFx(){return GameAssets.instance.GetDeathFx(deathFxName);}
    public GameObject GetDeathFxObj(){GameObject go=GameAssets.instance.GetVFX("Explosion");if(GameAssets.instance.GetDeathFx(deathFxName)!=null){go=GameAssets.instance.GetDeathFx(deathFxName).obj;}return go;}


    void LoadValues(){
        skinName=SaveSerial.instance.playerData.skinName;
        overlayColor=Color.HSVToRGB(SaveSerial.instance.playerData.overlayColor[0], SaveSerial.instance.playerData.overlayColor[1], SaveSerial.instance.playerData.overlayColor[2]);
        trailName=SaveSerial.instance.playerData.trailName;
        flaresName=SaveSerial.instance.playerData.flaresName;
        deathFxName=SaveSerial.instance.playerData.deathFxName;
        musicName=SaveSerial.instance.playerData.musicName;
    }
}
