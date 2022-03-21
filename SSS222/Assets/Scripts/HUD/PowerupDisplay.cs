using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerupDisplay : MonoBehaviour{
    [Header("Config")]
    [SerializeField] Image bg;
    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] Image highlightIMG;
    [Header("Values")]
    public int number=0;
    public string pwrup;
    public float value;

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
            }
            else{pwrup=Player.instance._curPwrupName();}

            if(txt!=null){
                float timer=Player.instance.powerupTimer;
                if(timer<10f&&timer>=0f){value=(float)System.Math.Round((float)timer, 1);txt.characterSpacing=-25f;}
                else if(timer>10f){value=(float)Mathf.RoundToInt(timer);txt.characterSpacing=0f;}
                else if(timer==-5f){value=-5f;}
                //var value=System.Math.Round(timer, 1);

                if(value<=0&&value>-5){value = 0;}
                if(value<=-5){txt.text="∞";}
                else{txt.text=value.ToString();}
            }
        }else{
            pwrup="null";
            img.color=Color.white;
        }
        string name="null";
        if(!String.IsNullOrEmpty(pwrup)){name=pwrup;}
        img.sprite=GameAssets.instance.Spr(name+"Pwrup");
        //if(pwrup=="null")color.a=0;
        //else color.a=1;
        if(number>=0&&Player.instance!=null&&Player.instance.powerupCurID==number){highlightIMG.enabled=true;}
        else{highlightIMG.enabled=false;}
    }
    public void SetPlayerPowerup(){if(Player.instance!=null)if(number>=0)Player.instance.powerupCurID=number;}
}