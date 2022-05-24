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
    [SerializeField] Color highlightTimerColor=new Color(40/255,25/255,250/255,60/255);
    [Header("Values")]
    public int number=0;
    public string pwrup;
    public int ammo;
    public float timer;
    public bool displayTimerTxt=false;

    Image img;
    Color bgcolor;
    void Start(){
        img=GetComponent<Image>();
        if(bg!=null)bgcolor=bg.color;
        if(GameRules.instance!=null){pwrup="";img.sprite=GameAssets.instance.Spr("nullPwrup");}
    }
    void Update(){
        if(Player.instance!=null){if(number<Player.instance.powerups.Length){
            var _id=number;
            if(number==-1&&Player.instance.powerups.Length==1)_id=0;
            if(_id>=0){
                if(Player.instance.GetPowerup(_id)!=null){pwrup=Player.instance.GetPowerup(_id).name;}
            }else{pwrup=Player.instance._curPwrupName();if(!GameRules.instance.displayCurrentPowerup){Destroy(gameObject);}}

            ammo=0;
            if(number>=0){ammo=Player.instance.GetPowerup(_id).ammo;}
            if(Player.instance._curPwrup()!=null)timer=Player.instance._curPwrup().timer;
            float timerMax=0;if(!String.IsNullOrEmpty(Player.instance._curPwrupName())){if(Player.instance.GetWeaponProperty(Player.instance._curPwrupName())!=null)timerMax=Player.instance.GetWeaponProperty(Player.instance._curPwrupName()).duration;}

            if(!displayTimerTxt){
                if(_id>=0){ammo=Player.instance.GetPowerup(_id).ammo;}
                else{if(Player.instance._curPwrup()!=null)ammo=Player.instance._curPwrup().ammo;}
                if(ammo<=0&&ammo>-5){numberDisplay.SetActive(false);}
                else{numberDisplay.SetActive(true);}
                
                if(_id>=0&&Player.instance!=null&&Player.instance.powerupCurID==_id){highlightIMG.enabled=true;}
                else{highlightIMG.enabled=false;}

                if(timer>0){highlightIMG.fillAmount=(timer/timerMax);highlightIMG.color=highlightTimerColor;}
                else{highlightIMG.fillAmount=1;highlightIMG.color=new Color(1,1,1,35f/255f);}
                if(txt!=null)txt.characterSpacing=0f;
            }else{
                if(timer<10f&&timer>=0f){timer=(float)System.Math.Round((float)timer,1);if(txt!=null)txt.characterSpacing=-25f;}
                else if(timer>10f){timer=(float)Mathf.RoundToInt(timer);if(txt!=null)txt.characterSpacing=0f;}

                if(_id>=0&&Player.instance!=null&&Player.instance.powerupCurID==_id){highlightIMG.enabled=true;highlightIMG.fillAmount=1;highlightIMG.color=new Color(1,1,1,35f/255f);}
                else{highlightIMG.enabled=false;}
            }
            if(txt!=null){
                if(!String.IsNullOrEmpty(pwrup)){
                    if(bg!=null)bg.color=bgcolor;
                    if((ammo<=0&&ammo>-5)||(displayTimerTxt&&timer<=0&&timer>-5)){txt.text="?";}
                    if((ammo==-5)||(displayTimerTxt&&timer>-5)){txt.text="∞";}
                    else{if(!displayTimerTxt){txt.text=ammo.ToString();}else{txt.text=timer.ToString();}}
                }else{txt.text="";if(bg!=null)bg.color=Color.clear;}
            }else{Debug.LogWarning("No txt obj assigned!");}
            string name=pwrup;
            if(!String.IsNullOrEmpty(name)){
                if(name.Contains(Player.instance._itemSuffix)){name=name.Split('_')[0];}name=name+"Pwrup";
                img.sprite=GameAssets.instance.Get(name).GetComponent<SpriteRenderer>().sprite;
            }else{img.sprite=GameAssets.instance.Spr("nullPwrup");}
        }}else{img.sprite=GameAssets.instance.Spr("nullPwrup");txt.text="";highlightIMG.fillAmount=0;}
    }
    public void SetPlayerPowerup(){if(Player.instance!=null)if(number>=0)Player.instance.powerupCurID=number;}
}