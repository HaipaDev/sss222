using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class BarValue : MonoBehaviour{
    [SerializeField] barType barType=barType.Fill;
    [SerializeField] string valueName;
    [SerializeField] float value;
    [SerializeField] float maxValue;
    [DisableInPlayMode][SerializeField] bool onlyOnEnable=false;
    [HideInPlayMode][SerializeField] bool onValidate=false;
    void Start(){if(onlyOnEnable)ChangeBar();}
    void OnEnable(){if(onlyOnEnable)ChangeBar();}
    void OnValidate(){if(onValidate)ChangeBar();}
    void Update(){if(!onlyOnEnable)ChangeBar();}
    void ChangeBar(){
        if(valueName=="health"){if(Player.instance!=null){value=Player.instance.health;maxValue=Player.instance.healthMax;}else{if(GameRules.instance!=null){value=GameRules.instance.healthPlayer;maxValue=GameRules.instance.healthMaxPlayer;}}}
        if(valueName=="energy"){if(Player.instance!=null){value=Player.instance.energy;maxValue=Player.instance.energyMax;}else{if(GameRules.instance!=null){value=GameRules.instance.energyPlayer;maxValue=GameRules.instance.energyMaxPlayer;}}}
        if(valueName=="hpAbsorp"){if(Player.instance!=null){value=Player.instance.hpAbsorpAmnt;maxValue=Player.instance.healthMax/GameRules.instance.hpAbsorpFractionCap;}}
        if(valueName=="enAbsorp"){if(Player.instance!=null){value=Player.instance.enAbsorpAmnt;maxValue=Player.instance.energyMax/GameRules.instance.enAbsorpFractionCap;}}
        if(valueName=="xp"){
            if(GameSession.instance!=null){
                if(GameSession.instance.xp>GameSession.instance.xpMax){value=GameSession.instance.xp;maxValue=GameSession.instance.xpMax*GameRules.instance.maxXpOvefillMult;GetComponent<Image>().sprite=GameAssets.instance.Spr("overfilledXpBar");}
                else{value=GameSession.instance.xp;maxValue=GameSession.instance.xpMax;GetComponent<Image>().sprite=GameAssets.instance.Spr("regularXpBar");}
            }
        }
        if(valueName=="shopTimer"){if(Shop.instance!=null){value=Shop.instance.shopTimer;maxValue=Shop.instance.shopTimeMax;}}

        if(GameRules.instance!=null){
            EnemyClass _en=null;
            if(valueName.Contains("EnemySB")){if(SandboxCanvas.instance!=null){
                if(!String.IsNullOrEmpty(SandboxCanvas.instance.enemyToModify))_en=Array.Find(GameRules.instance.enemies,x=>x.name==SandboxCanvas.instance.enemyToModify);}}
            if(_en!=null){
                if(valueName=="healthEnemySB"){value=_en.healthStart;maxValue=_en.healthMax;}
            }
        }

        if(barType==barType.HorizontalR){transform.localScale=new Vector2(value/maxValue,transform.localScale.y);}
        if(barType==barType.HorizontalL){transform.localScale=new Vector2(value/maxValue,transform.localScale.y);/*new Vector2(-(value/maxValue),transform.localScale.y);*/}
        if(barType==barType.VerticalU){transform.localScale=new Vector2(transform.localScale.x,-(value/maxValue));}
        if(barType==barType.VerticalD){transform.localScale=new Vector2(transform.localScale.x,value/maxValue);}
        if(barType==barType.Fill){GetComponent<Image>().fillAmount=value/maxValue;}
    }
}
public enum barType{
    HorizontalR,
    HorizontalL,
    VerticalU,
    VerticalD,
    Fill
}