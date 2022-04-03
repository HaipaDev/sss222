using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class SliderValue : MonoBehaviour{
    [SerializeField] public string value="masterVolume";
    [DisableInPlayMode][SerializeField] bool onlyOnEnable=false;
    void Start(){if(onlyOnEnable)ChangeSlider();}
    void OnEnable(){if(onlyOnEnable)ChangeSlider();}
    void Update(){if(!onlyOnEnable)ChangeSlider();}

    void ChangeSlider(){
        float _val=0f;
        var s=SaveSerial.instance;  var ss=SaveSerial.instance.settingsData;
        SettingsMenu sm=null;if(SettingsMenu.instance!=null)sm=SettingsMenu.instance;
        var gr=GameRules.instance;var sb=SandboxCanvas.instance;    EnemyClass _en=null;
        if(gr!=null){
            switch(value){
                case "powerupsCapacity":_val=gr.powerupsCapacity;break;
                case "gameSpeed":_val=gr.defaultGameSpeed;break;
            }
            if(sb!=null){
                if(value.Contains("EnemySB")){if(!System.String.IsNullOrEmpty(sb.enemyToModify))_en=System.Array.Find(gr.enemies,x=>x.name==sb.enemyToModify);}
                switch(value){
                    case "bgHueSB":_val=sb.bgHue;break;
                    case "bgSaturSB":_val=sb.bgSatur;break;
                    case "bgValueSB":_val=sb.bgValue;break;
                    //case "spriteHueEnemySB":_val=_en.sprHue;break;
                }
            }
        }
        GetComponent<Slider>().value=_val;
    }
}
