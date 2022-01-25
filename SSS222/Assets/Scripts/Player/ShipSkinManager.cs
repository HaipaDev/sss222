using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ShipSkinManager : MonoBehaviour{
    public static ShipSkinManager instance;
    [HeaderAttribute("Properties")]
    public string skinName="Mk.22";
    [SceneObjectsOnly][SerializeField] public GameObject overlayObj;
    public Color overlayColor=Color.red;
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
            overlayObj.transform.localScale=Vector2.one;
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
    }


    //string GetSkinName(){string str=skinName;if(skinName.Contains(" _")){str=skinName.Split('_')[0];}return str;}
    GSkin GetSkin(string str){string _str=str;if(_str.Contains("_")){_str=_str.Split('_')[0];}return GameAssets.instance.GetSkin(_str);}
    GSkin GetSkinCurrent(){string _str=skinName;if(_str.Contains("_")){_str=_str.Split('_')[0];}return GameAssets.instance.GetSkin(_str);}
    GSkinVariant GetSkinVariant(string str,int id){return GameAssets.instance.GetSkinVariant(str,id);}
    public Sprite GetSkinSprite(string str){Sprite spr=null;
        if(str.Contains("_")){spr=GameAssets.instance.GetSkin(str.Split('_')[0]).variants[int.Parse(str.Split('_')[1])].spr;}
        else{spr=GameAssets.instance.GetSkin(str).spr;}
    return spr;}
    public Sprite GetOverlaySprite(string str){Sprite spr=null;
        if(str.Contains("_")){spr=GameAssets.instance.GetSkin(str.Split('_')[0]).variants[int.Parse(str.Split('_')[1])].sprOverlay;}
        else{spr=GameAssets.instance.GetSkin(str).sprOverlay;}
    return spr;}
    void SetSkin(string str){if(spr!=null){spr.sprite=GetSkinSprite(str);}else if(img!=null){img.sprite=GetSkinSprite(str);}}
    void SetOverlay(Sprite sprite, Color color){
        Color _color=Color.white;if(color!=Color.clear){_color=color;}
        if(overlaySpr!=null){
            overlaySpr.sprite=sprite;
            overlaySpr.color=_color;
        }else if(overlayImg!=null){
            overlayImg.sprite=sprite;
            overlayImg.color=_color;
        }
    }
    void LoadValues(){
        skinName=SaveSerial.instance.playerData.skinName;
        overlayColor=Color.HSVToRGB(SaveSerial.instance.playerData.overlayColor[0], SaveSerial.instance.playerData.overlayColor[1], SaveSerial.instance.playerData.overlayColor[2]);
    }
}
