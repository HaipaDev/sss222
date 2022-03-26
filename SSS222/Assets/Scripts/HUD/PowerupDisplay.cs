using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class PowerupDisplay : MonoBehaviour{
    [Header("Config")]
    [SceneObjectsOnly][SerializeField] GameObject numberDisplay;
    [SerializeField] Image bg;
    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] Image highlightIMG;
    [SerializeField] Color highlightTimerColor;
    [Header("Values")]
    public int number=0;
    public string pwrup;
    public int ammo;
    public float timer;
    public bool displayTimerTxt=false;

    Image img;
    Color color;
    void Start(){
        //if(Player.instance!=null)pwrup = Player.instance.powerup;
        img=GetComponent<Image>();
        //if(powerups!=true){
        //if(powerups!=true||(Player.instance!=null&&Player.instance.weaponsLimited)){
        if(transform.GetComponentInChildren<Image>()!=null){bg=transform.GetComponentInChildren<Image>();}
        //}
        if(bg!=null)color=bg.color;
        if(GameRules.instance!=null){pwrup="null";img.sprite=GameAssets.instance.Spr(pwrup+"Pwrup");}
    }
    void Update(){
        if(bg!=null)bg.color=color;

        if(Player.instance!=null){
            if(number>=0){
                if(Player.instance.GetPowerup(number)!=null){pwrup=Player.instance.GetPowerup(number).name;}
            }else{pwrup=Player.instance._curPwrupName();}

            ammo=0;
            if(number>=0){ammo=Player.instance.GetPowerup(number).ammo;}
            timer=Player.instance._curPwrup().timer;
            float timerMax=0;if(!String.IsNullOrEmpty(Player.instance._curPwrupName())){if(Player.instance.GetWeaponProperty(Player.instance._curPwrupName())!=null)timerMax=Player.instance.GetWeaponProperty(Player.instance._curPwrupName()).duration;}

            if(!displayTimerTxt){
                if(number>=0){ammo=Player.instance.GetPowerup(number).ammo;}
                else{ammo=Player.instance._curPwrup().ammo;}
                if(ammo<=0&&ammo>-5){numberDisplay.SetActive(false);}
                else{numberDisplay.SetActive(true);}
                
                if(number>=0&&Player.instance!=null&&Player.instance.powerupCurID==number){highlightIMG.enabled=true;}
                else{highlightIMG.enabled=false;}

                if(timer>0){highlightIMG.fillAmount=(timer/timerMax);highlightIMG.color=highlightTimerColor;}
                else{highlightIMG.fillAmount=1;highlightIMG.color=new Color(1,1,1,0.6f);}
            }else{
                if(timer<10f&&timer>=0f){timer=(float)System.Math.Round((float)timer,1);if(txt!=null)txt.characterSpacing=-25f;}
                else if(timer>10f){timer=(float)Mathf.RoundToInt(timer);if(txt!=null)txt.characterSpacing=0f;}

                if(number>=0&&Player.instance!=null&&Player.instance.powerupCurID==number){highlightIMG.enabled=true;highlightIMG.fillAmount=1;highlightIMG.color=new Color(1,1,1,0.6f);}
                else{highlightIMG.enabled=false;}
            }
            if(txt!=null){
                if((ammo<=0&&ammo>-5)||(displayTimerTxt&&timer<=0&&timer>-5)){txt.text="?";}
                if((ammo==-5)||(displayTimerTxt&&timer>-5)){txt.text="∞";}
                else{if(!displayTimerTxt){txt.text=ammo.ToString();}else{txt.text=timer.ToString();}}
            }
            string name=pwrup;
            if(!String.IsNullOrEmpty(name)){
                if(name.Contains(Player.instance._itemSuffix)){name=name.Split('_')[0];}name=name+"Pwrup";
                img.sprite=GameAssets.instance.Spr(name);
            }else{img.sprite=GameAssets.instance.Spr("nullPwrup");}
        }else{img.sprite=GameAssets.instance.Spr("nullPwrup");}
    }
    public void SetPlayerPowerup(){if(Player.instance!=null)if(number>=0)Player.instance.powerupCurID=number;}
}