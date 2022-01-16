using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ShipSkinManager : MonoBehaviour{
    [AssetsOnly][SerializeField] GameObject overlayPrefab;
    [HeaderAttribute("Properties")]
    public string skinName="Mk.22";
    [SerializeField] Color overlayColor=Color.red;
    GameObject overlayOBJ;
    SpriteRenderer overlaySpr;
    Image overlayImg;
    SpriteRenderer spr;
    Image img;
    IEnumerator Start(){
        spr=GetComponent<SpriteRenderer>();
        if(spr==null)img=GetComponent<Image>();
        LoadValues();
        if(GetSkin(skinName).sprOverlay!=null){
            overlayOBJ=Instantiate(overlayPrefab,new Vector2(transform.position.x,transform.position.y),Quaternion.identity,transform);
            overlayOBJ.transform.position=new Vector3(overlayOBJ.transform.position.x,overlayOBJ.transform.position.y,-0.01f);
            overlayOBJ.transform.localScale=Vector2.one;
            overlaySpr=overlayOBJ.GetComponent<SpriteRenderer>();
            if(overlaySpr==null){overlayImg=overlayOBJ.GetComponent<Image>();}
            if(GameSession.maskMode!=0&&overlaySpr!=null){overlaySpr.maskInteraction=(SpriteMaskInteraction)GameSession.maskMode;}
        }

        yield return new WaitForSeconds(0.05f);
        SetSkin(skinName);
        Color color=Color.white;
        if(skinName.Contains("Chameleon")){color=overlayColor;}
        if(GetSkin(skinName).sprOverlay!=null)SetOverlay(GetSkin(skinName).sprOverlay,color);
    }

    //void Update(){}
    GSkin GetSkin(string str){return GameAssets.instance.GetSkin(str);}
    void SetSkin(string str){if(spr!=null){spr.sprite=GetSkin(str).spr;}else if(img!=null){img.sprite=GetSkin(str).spr;}}
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
