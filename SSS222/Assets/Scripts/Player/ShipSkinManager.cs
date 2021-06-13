using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSkinManager : MonoBehaviour{
    public int skinID=0;
    public bool addOverlay = false;
    [SerializeField] Sprite[] skins;
    [SerializeField] Sprite[] overlays;
    [SerializeField] GameObject overlayPrefab;
    [HeaderAttribute("Properties")]
    [SerializeField] int chameleonOverlayID;
    [SerializeField] Color chameleonOvColor;
    
    //[SerializeField] int chameleonOvAlpha;
    GameObject overlayOBJ;
    SpriteRenderer overlay;
    SpriteRenderer spr;
    SaveSerial saveSerial;
    Player player;
    void Start(){
        player=GetComponent<Player>();
        spr=GetComponent<SpriteRenderer>();
        saveSerial=FindObjectOfType<SaveSerial>();
        LoadValues();
        //if(skinID<1){skinID=1;}
        skins=FindObjectOfType<GameAssets>().skins;
        overlays=FindObjectOfType<GameAssets>().skinOverlays;
        if(skinID==1){addOverlay=true;}
        if(addOverlay==true){
            overlayOBJ=Instantiate(overlayPrefab,new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
            overlayOBJ.transform.parent = gameObject.transform;
            overlayOBJ.transform.position=new Vector3(overlayOBJ.transform.position.x,overlayOBJ.transform.position.y,-0.01f);
            overlayOBJ.transform.localScale=Vector2.one;
            overlay=overlayOBJ.GetComponent<SpriteRenderer>();
            if(GameSession.maskMode!=0)overlayOBJ.GetComponent<SpriteRenderer>().maskInteraction=(SpriteMaskInteraction)GameSession.maskMode;
        }
    }

    void Update(){
        if(skinID==1){
            SetSkin(skins[skinID]);
            SetOverlay(overlays[chameleonOverlayID],chameleonOvColor);
            //chameleonOvColor.a = chameleonOvAlpha;
        }else{
            SetSkin(skins[skinID]);
        }
    }
    void SetSkin(Sprite sprite){
        if(spr!=null)spr.sprite=sprite;
    }
    void SetOverlay(Sprite sprite, Color color){
        if(overlay!=null){
        overlay.sprite=sprite;
        overlay.color=color;
        }
    }
    void LoadValues(){
        skinID=saveSerial.playerData.skinID;
        chameleonOvColor = Color.HSVToRGB(saveSerial.playerData.chameleonColor[0], saveSerial.playerData.chameleonColor[1], saveSerial.playerData.chameleonColor[2]);
    }
}
