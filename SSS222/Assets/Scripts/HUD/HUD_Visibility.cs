using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Visibility : MonoBehaviour{
    [SerializeField] HUDVis_type type;
    [SerializeField] bool overwriteSav;
    float savAlpha;
    void Start(){
        savAlpha=GetTransparency();
    }
    void Update(){
        if(type==HUDVis_type.graphics){SetTransparency(SaveSerial.instance.settingsData.hudVis_graphics);}
        if(type==HUDVis_type.text){SetTransparency(SaveSerial.instance.settingsData.hudVis_text);}
        if(type==HUDVis_type.barFill){SetTransparency(SaveSerial.instance.settingsData.hudVis_barFill);}
        if(type==HUDVis_type.absorpFill){SetTransparency(SaveSerial.instance.settingsData.hudVis_absorpFill);}
        if(type==HUDVis_type.popups){SetTransparency(SaveSerial.instance.settingsData.hudVis_popups);}
        if(type==HUDVis_type.notif){SetTransparency(SaveSerial.instance.settingsData.hudVis_notif);}
    }
    void SetTransparency(float amnt){
        if(GetComponent<Image>()!=null){var img=GetComponent<Image>();var tempColor=img.color;
          if(overwriteSav)tempColor.a=amnt;
          else tempColor.a=savAlpha*amnt;
          img.color=tempColor;}
        if(GetComponent<TMPro.TextMeshProUGUI>()!=null){var txt=GetComponent<TMPro.TextMeshProUGUI>();
          if(overwriteSav)txt.alpha=amnt;
          else txt.alpha=savAlpha*amnt;}
    }
    float GetTransparency(){
        if(GetComponent<Image>()!=null){return GetComponent<Image>().color.a;}
        if(GetComponent<TMPro.TextMeshProUGUI>()!=null){return GetComponent<TMPro.TextMeshProUGUI>().alpha;}
        else return 0;
    }
}
enum HUDVis_type{
    graphics,text,barFill,absorpFill,popups,notif
}
