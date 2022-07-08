using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour{
    Sprite HPBarNormal;
    [SerializeField] Sprite HPBarGold;
    void Start(){
        HPBarNormal=GetComponent<Image>().sprite;
    }
    void Update(){
        if(Player.instance!=null){
            GetComponent<Image>().fillAmount=(Player.instance.health/Player.instance.healthMax);
            if(Player.instance._hasStatus("gclover")){GetComponent<Image>().sprite=HPBarGold;}
            else{GetComponent<Image>().sprite=HPBarNormal;}
        }else{if(GameRules.instance!=null){
            GetComponent<Image>().fillAmount=/*0;*/(GameRules.instance.healthPlayer/GameRules.instance.healthMaxPlayer);
            //if(gclover==true){GetComponent<Image>().sprite=HPBarGold;}
            //else{GetComponent<Image>().sprite=HPBarNormal;}
        }}
    }
}
