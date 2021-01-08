using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour{
    TMPro.TextMeshProUGUI txt;
    Player player;
    void Start(){
        txt=GetComponent<TMPro.TextMeshProUGUI>();
        player=FindObjectOfType<Player>();
    }

    void Update(){
        if(player.ammo>0){txt.alpha=1;txt.text=player.ammo.ToString();}
        else{txt.alpha=0;}
    }
}
