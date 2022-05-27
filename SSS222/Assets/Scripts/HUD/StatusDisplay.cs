using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusDisplay : MonoBehaviour{
    [Header("Config")]
    [SerializeField] Image bg;
    [SerializeField] TextMeshProUGUI txt;
    [Header("Values")]
    public int number=0;
    public string state="";
    public float value;

    Image img;
    Color color;
    void Start(){
        img=GetComponent<Image>();
        if(bg!=null)color=bg.color;
    }
    void Update(){
        if(bg!=null)bg.color=color;

        
        if(Player.instance!=null){
            if(Player.instance.statuses.Count>number){if(Player.instance.statuses[number].name!=""){state=Player.instance.statuses[number].name;}else state="";}else state="";
            /*if(number==1){state=Player.instance.status1;}
            if(number==2){state=Player.instance.status2;}
            if(number==3){state=Player.instance.status3;}*/
            //var s=this.GetType().GetField(state+"Sprite").SetValue(this,false);
            //img.sprite=s;
            if(String.IsNullOrEmpty(state)){
                Destroy(gameObject);
                /*color.a = 0f;
                color.a = 0f;
                txt.text = "";*/
            }else{
                //color.a = 1f;
                color.a = 130/255f;
                /*var sprr=this.GetType().GetField(state+"Sprite").GetValue(this);
                this.GetType().GetField("sprite").SetValue(this,sprr);
                img.sprite=sprite;*/
                if(GameAssets.instance!=null)img.sprite=GameAssets.instance.Get(state+"Pwrup").GetComponent<SpriteRenderer>().sprite;
                var timer=Player.instance.GetStatus(state).timer;//Player.instance.GetType().GetField(state+"Timer").GetValue(Player.instance);
                if((float)timer<10f&&(float)timer>=0f){value=(float)System.Math.Round((float)timer, 1);txt.characterSpacing=-25f;}
                else if((float)timer>10f){value=(float)Mathf.RoundToInt((float)timer);txt.characterSpacing=0f;}
                else if((float)timer==-5f){value=-5f;}
                //var value=System.Math.Round(timer, 1);

                if (value<=0&&value>-5) {value = 0;}
                if(value<=-5){txt.text="∞";}
                else {txt.text=value.ToString();}
            }
        }
    }
}