using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour{
    [HideInInspector]public bool gclover;
    Sprite HPBarNormal;
    [SerializeField] Sprite HPBarGold;
    void Start(){
        HPBarNormal=GetComponent<Image>().sprite;
    }
    void Update(){
        if(Player.instance!=null){
            GetComponent<Image>().fillAmount=(Player.instance.health/Player.instance.healthMax);
            if(gclover==true){GetComponent<Image>().sprite=HPBarGold;}
            else{GetComponent<Image>().sprite=HPBarNormal;}
        }else{if(GameRules.instance!=null){
            GetComponent<Image>().fillAmount=0;//(GameRules.instance.healthPlayer/GameRules.instance.healthMaxPlayer);
            //if(gclover==true){GetComponent<Image>().sprite=HPBarGold;}
            //else{GetComponent<Image>().sprite=HPBarNormal;}
        }}
    }
}
