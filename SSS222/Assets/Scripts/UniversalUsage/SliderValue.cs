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
        var gr=GameRules.instance;
        var s=SaveSerial.instance;  var ss=SaveSerial.instance.settingsData;
        SettingsMenu sm=null;if(SettingsMenu.instance!=null)sm=SettingsMenu.instance;
        if(gr!=null){switch(value){
            case "powerupsCapacity":_val=gr.powerupsCapacity;break;
            case "gameSpeed":_val=gr.defaultGameSpeed;break;
        }}
        GetComponent<Slider>().value=_val;
    }
}
