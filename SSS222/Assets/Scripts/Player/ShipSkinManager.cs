using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ShipSkinManager : MonoBehaviour{
    [AssetsOnly][SerializeField] GameObject overlayPrefab;
    [HeaderAttribute("Properties")]
    public string skinName="Mk.22";
    [SerializeField] Color chameleonOvColor=Color.red;
    
    //[SerializeField] int chameleonOvAlpha;
    GameObject overlayOBJ;
    SpriteRenderer overlay;
    SpriteRenderer spr;
    SaveSerial saveSerial;
    Player player;
    IEnumerator Start(){
        player=GetComponent<Player>();
        spr=GetComponent<SpriteRenderer>();
        saveSerial=FindObjectOfType<SaveSerial>();
        LoadValues();
        if(GetSkin(skinName).sprOverlay!=null){
            overlayOBJ=Instantiate(overlayPrefab,new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
            overlayOBJ.transform.parent = gameObject.transform;
            overlayOBJ.transform.position=new Vector3(overlayOBJ.transform.position.x,overlayOBJ.transform.position.y,-0.01f);
            overlayOBJ.transform.localScale=Vector2.one;
            overlay=overlayOBJ.GetComponent<SpriteRenderer>();
            if(GameSession.maskMode!=0)overlayOBJ.GetComponent<SpriteRenderer>().maskInteraction=(SpriteMaskInteraction)GameSession.maskMode;
        }

        yield return new WaitForSeconds(0.05f);
        SetSkin(skinName);
        Color color=Color.white;
        if(skinName.Contains("Chameleon")){color=chameleonOvColor;}
        if(GetSkin(skinName).sprOverlay!=null)SetOverlay(GetSkin(skinName).sprOverlay,color);
    }

    //void Update(){}
    GSkin GetSkin(string str){return GameAssets.instance.GetSkin(str);}
    void SetSkin(string str){if(this.spr!=null)this.spr.sprite=GetSkin(str).spr;}
    void SetOverlay(Sprite sprite, Color color){
        if(overlay!=null){
        overlay.sprite=sprite;
        overlay.color=color;
        }
    }
    void LoadValues(){
        skinName=saveSerial.playerData.skinName;
        chameleonOvColor=Color.HSVToRGB(saveSerial.playerData.chameleonColor[0], saveSerial.playerData.chameleonColor[1], saveSerial.playerData.chameleonColor[2]);
    }
}
