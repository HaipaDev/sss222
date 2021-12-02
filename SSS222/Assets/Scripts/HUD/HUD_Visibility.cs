using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Visibility : MonoBehaviour{
    [SerializeField] HUDVis_type type;
    [SerializeField] bool overwriteSav;
    [SerializeField] float animVal;
    float savAlpha;
    public float alphaVal;
    void Start(){
        savAlpha=GetTransparency();
    }
    void Update(){
        if(type==HUDVis_type.graphics){SetAlphaVal(SaveSerial.instance.settingsData.hudVis_graphics);}
        if(type==HUDVis_type.text){SetAlphaVal(SaveSerial.instance.settingsData.hudVis_text);}
        if(type==HUDVis_type.barFill){SetAlphaVal(SaveSerial.instance.settingsData.hudVis_barFill);}
        if(type==HUDVis_type.absorpFill){SetAlphaVal(SaveSerial.instance.settingsData.hudVis_absorpFill);}
        if(type==HUDVis_type.popups){SetAlphaVal(SaveSerial.instance.settingsData.hudVis_popups);}
        if(type==HUDVis_type.notif){SetAlphaVal(SaveSerial.instance.settingsData.hudVis_notif);}
        //Skip if any other components should override
        if(GetComponent<AmmoDisplay>()==null){
        SetTrapnsparency();}
    }
    void SetAlphaVal(float amnt){
        if(overwriteSav)alphaVal=amnt;
        else if(type==HUDVis_type.popups)alphaVal=animVal*amnt;
        else alphaVal=savAlpha*amnt;
    }
    void SetTrapnsparency(){
        if(GetComponent<Image>()!=null){var img=GetComponent<Image>();var tempColor=img.color;
        tempColor.a=alphaVal;
        img.color=tempColor;
        }
        else if(GetComponent<TMPro.TextMeshProUGUI>()!=null){var txt=GetComponent<TMPro.TextMeshProUGUI>();
        txt.alpha=alphaVal;
        }
    }
    float GetTransparency(){
        if(GetComponent<Image>()!=null){return GetComponent<Image>().color.a;}
        else if(GetComponent<TMPro.TextMeshProUGUI>()!=null){return GetComponent<TMPro.TextMeshProUGUI>().alpha;}
        else return 0;
    }
}
enum HUDVis_type{
    graphics,text,barFill,absorpFill,popups,notif
}
