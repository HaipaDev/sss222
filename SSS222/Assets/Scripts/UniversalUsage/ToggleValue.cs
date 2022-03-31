using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleValue : MonoBehaviour{
    [SerializeField] public string value="shopOn";
    //[SerializeField] bool changeOnValidate;

    Toggle toggle;
    Image img;
    void Start(){
        toggle=GetComponent<Toggle>();
        img=GetComponent<Image>();
    }

    void Update(){ChangeText();}

    void ChangeText(){
        var gr=GameRules.instance;
        var s=SaveSerial.instance;
        var ss=SaveSerial.instance.settingsData;
        SettingsMenu sm=null;if(SettingsMenu.instance!=null)sm=SettingsMenu.instance;
        int _id=0;
        if(gr!=null){switch(value){
            case("crystalsOn"):toggle.isOn=gr.crystalsOn;break;
            case("coresOn"):toggle.isOn=gr.coresOn;break;
            case("xpOn"):toggle.isOn=gr.xpOn;break;
            case("shopOn"):toggle.isOn=gr.shopOn;break;
            case("shopCargoOn"):toggle.isOn=gr.shopCargoOn;break;
            case("iteminvOn"):toggle.isOn=gr.iteminvOn;break;
            case("statUpgOn"):toggle.isOn=gr.statUpgOn;break;
            case("modulesOn"):toggle.isOn=gr.modulesOn;break;
            case("levelingOn"):toggle.isOn=gr.levelingOn;break;
            case("barrierOn"):toggle.isOn=gr.barrierOn;break;
            case("moveX"):toggle.isOn=gr.moveX;break;
            case("moveY"):toggle.isOn=gr.moveY;break;
            case("moveAxis"):_id=0;if(gr.moveX&&!gr.moveY){_id=1;}else if(!gr.moveX&&gr.moveY){_id=2;}img.sprite=GetComponent<SpritesLib>().sprites[_id];break;
        }}
        if(sm!=null){switch(value){
            case("steering"):_id=0;if(ss.inputType==InputType.keyboard){_id=1;}else if(ss.inputType==InputType.touch){_id=2;}else if(ss.inputType==InputType.drag){_id=3;}img.sprite=GetComponent<SpritesLib>().sprites[_id];break;
        }}
    }
}
