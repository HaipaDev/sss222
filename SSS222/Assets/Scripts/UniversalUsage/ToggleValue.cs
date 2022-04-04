using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ToggleValue : MonoBehaviour{
    [SerializeField] public string value="shopOn";
    [DisableInPlayMode][SerializeField] bool onlyOnEnable=false;
    [HideInPlayMode][SerializeField] bool onValidate=false;
    void Start(){if(onlyOnEnable)ChangeToggle();}
    void OnEnable(){if(onlyOnEnable)ChangeToggle();}
    void OnValidate(){if(onValidate)ChangeToggle();}
    void Update(){if(!onlyOnEnable)ChangeToggle();}

    void ChangeToggle(){
        var gr=GameRules.instance;
        var s=SaveSerial.instance;
        var ss=SaveSerial.instance.settingsData;
        SettingsMenu sm=null;if(SettingsMenu.instance!=null)sm=SettingsMenu.instance;
        int _id=0;Sprite _spr=null;bool? _b=null;
        if(gr!=null){switch(value){
            case("crystalsOn"):_b=gr.crystalsOn;break;
            case("coresOn"):_b=gr.coresOn;break;
            case("xpOn"):_b=gr.xpOn;break;
            case("shopOn"):_b=gr.shopOn;break;
            case("shopCargoOn"):_b=gr.shopCargoOn;break;
            case("iteminvOn"):_b=gr.iteminvOn;break;
            case("statUpgOn"):_b=gr.statUpgOn;break;
            case("modulesOn"):_b=gr.modulesOn;break;
            case("levelingOn"):_b=gr.levelingOn;break;
            case("barrierOn"):_b=gr.barrierOn;break;
            case("moveX"):_b=gr.moveX;break;
            case("moveY"):_b=gr.moveY;break;
            case("moveAxis"):_id=0;if(gr.moveX&&!gr.moveY){_id=1;}else if(!gr.moveX&&gr.moveY){_id=2;}_spr=GetComponent<SpritesLib>().sprites[_id];break;
            case("autoshoot"):_b=gr.autoShootPlayer;break;
        }}
        if(sm!=null){switch(value){
            case("steering"):_id=0;if(ss.inputType==InputType.keyboard){_id=1;}else if(ss.inputType==InputType.touch){_id=2;}else if(ss.inputType==InputType.drag){_id=3;}_spr=GetComponent<SpritesLib>().sprites[_id];break;
        }}
        if(_spr!=null)GetComponent<Image>().sprite=_spr;
        if(_b.HasValue)GetComponent<Toggle>().isOn=(bool)_b;
    }
}
