using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour{
    [SerializeField] public string value="masterVolume";
    Slider slider;
    void Start(){
        slider=GetComponent<Slider>();
    }
    void Update(){ChangeSlider();}

    void ChangeSlider(){
        var gr=GameRules.instance;
        var s=SaveSerial.instance;
        var ss=SaveSerial.instance.settingsData;
        SettingsMenu sm=null;if(SettingsMenu.instance!=null)sm=SettingsMenu.instance;
        if(gr!=null){switch(value){
            case "powerupsCapacity":slider.value=gr.powerupsCapacity;break;
        }}
    }
}
