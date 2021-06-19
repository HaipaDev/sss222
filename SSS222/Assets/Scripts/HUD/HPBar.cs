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
        //if(GameSession.instance.gameModeSelected!=Array.FindIndex(GameCreator.instance.gamerulesetsPrefabs,e => e.cfgName.Contains("Classic")))
        GetComponent<Image>().fillAmount=(Player.instance.health/Player.instance.maxHP);
        /*else{
            if(Player.instance.health==0){GetComponent<Image>().fillAmount=0;}
            if(Player.instance.health==0.5f){GetComponent<Image>().fillAmount=0.17f;}
            if(Player.instance.health==1){GetComponent<Image>().fillAmount=0.236f;}
            if(Player.instance.health==1.5f){GetComponent<Image>().fillAmount=0.4f;}
            if(Player.instance.health==2){GetComponent<Image>().fillAmount=0.47f;}
            if(Player.instance.health==2.5f){GetComponent<Image>().fillAmount=0.63f;}
            if(Player.instance.health==3){GetComponent<Image>().fillAmount=0.7f;}
            if(Player.instance.health==3.5f){GetComponent<Image>().fillAmount=0.85f;}
            if(Player.instance.health==4){GetComponent<Image>().fillAmount=1;}
        }*/
        if(gclover==true){GetComponent<Image>().sprite=HPBarGold;}
        else{GetComponent<Image>().sprite=HPBarNormal;}
    }
    }
}
