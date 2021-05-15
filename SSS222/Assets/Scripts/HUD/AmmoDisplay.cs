using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour{
    TMPro.TextMeshProUGUI txt;
    void Start(){
        txt=GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update(){
        if(Player.instance.ammo>0){txt.alpha=1;txt.text=Player.instance.ammo.ToString();}
        else{txt.alpha=0;}
    }
}
