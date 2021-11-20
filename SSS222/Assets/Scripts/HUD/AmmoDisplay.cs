using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour{
    TMPro.TextMeshProUGUI txt;
    void Start(){txt=GetComponent<TMPro.TextMeshProUGUI>();}

    void Update(){
        if(Player.instance.ammoOn&&Player.instance.ammo>0){
            txt.text=Player.instance.ammo.ToString();
            if(GetComponent<HUD_Visibility>()!=null){if(GetComponent<HUD_Visibility>().enabled){txt.alpha=GetComponent<HUD_Visibility>().alphaVal;}
            else txt.alpha=1;}else txt.alpha=1;
        }
        else{txt.alpha=0;}
    }
}
