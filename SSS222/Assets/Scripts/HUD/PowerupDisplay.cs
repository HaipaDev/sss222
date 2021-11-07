using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupDisplay : MonoBehaviour{
    [SerializeField] public int number;
    float value;
    [SerializeField] bool powerups = true;
    [SerializeField] GameObject textObj;
    string pwrup;
    public string state="";

    Image img;
    TMPro.TextMeshProUGUI TMP;

    Color color;
    Color color2;
    Image bg;
    void Start(){
        //if(Player.instance!=null)pwrup = Player.instance.powerup;
        img=GetComponent<Image>();
        //if(powerups!=true){
        //if(powerups!=true||(Player.instance!=null&&Player.instance.weaponsLimited)){
        if(textObj==null){if(transform.GetComponentInChildren<TMPro.TextMeshProUGUI>()!=null)textObj=transform.GetComponentInChildren<TMPro.TextMeshProUGUI>().gameObject;}
        if(textObj!=null){TMP=textObj.GetComponent<TMPro.TextMeshProUGUI>();}
        if(transform.GetChild(0)!=null){bg=transform.GetChild(0).GetComponent<Image>();}
        //}
        //color=img.color;
        if(bg!=null)color2=bg.color;
        if(GameRules.instance!=null){pwrup=GameRules.instance.powerupStarting;img.sprite=GameAssets.instance.Spr(pwrup+"Pwrup");}
    }
    void Update(){
        //img.color=color;
        if(bg!=null)bg.color=color2;

        if(powerups==true){
            if(Player.instance!=null){
                pwrup=Player.instance.powerup;
                if(TMP!=null){
                    float timer=Player.instance.powerupTimer;
                    if(timer<10f&&timer>=0f){value=(float)System.Math.Round((float)timer, 1);TMP.characterSpacing=-25f;}
                    else if(timer>10f){value=(float)Mathf.RoundToInt(timer);TMP.characterSpacing=0f;}
                    else if(timer==-5f){value=-5f;}
                    //var value=System.Math.Round(timer, 1);

                    if (value<=0&&value>-5) {value = 0;}
                    if(value<=-5){TMP.text="∞";}
                    else {TMP.text=value.ToString();}
                }
            }else{
                pwrup=GameRules.instance.powerupStarting;
                img.color=Color.white;
            }
            string name=null;
            if(pwrup!=null)name=pwrup;
            if(pwrup.Contains("A")&&name!=null){name=pwrup.Trim('A');}
            img.sprite=GameAssets.instance.Spr(name+"Pwrup");
            if(pwrup=="null")color.a=0;
            else color.a=1;
        }else{
            if(Player.instance!=null){
                if(Player.instance.statuses.Count>number){if(Player.instance.statuses[number]!=""){state=Player.instance.statuses[number];}else state="";}else state="";
                /*if(number==1){state=Player.instance.status1;}
                if(number==2){state=Player.instance.status2;}
                if(number==3){state=Player.instance.status3;}*/
                //var s=this.GetType().GetField(state+"Sprite").SetValue(this,false);
                //img.sprite=s;
                if(state==""||state==null){
                    Destroy(gameObject);
                    /*color.a = 0f;
                    color2.a = 0f;
                    TMP.text = "";*/
                }else{
                    color.a = 1f;
                    color2.a = 130/255f;
                    /*var sprr=this.GetType().GetField(state+"Sprite").GetValue(this);
                    this.GetType().GetField("sprite").SetValue(this,sprr);
                    img.sprite=sprite;*/
                    if(GameAssets.instance!=null)img.sprite=GameAssets.instance.Spr(state+"Pwrup");
                    var timer=Player.instance.GetType().GetField(state+"Timer").GetValue(Player.instance);
                    if((float)timer<10f&&(float)timer>=0f){value=(float)System.Math.Round((float)timer, 1);TMP.characterSpacing=-25f;}
                    else if((float)timer>10f){value=(float)Mathf.RoundToInt((float)timer);TMP.characterSpacing=0f;}
                    else if((float)timer==-5f){value=-5f;}
                    //var value=System.Math.Round(timer, 1);

                    if (value<=0&&value>-5) {value = 0;}
                    if(value<=-5){TMP.text="∞";}
                    else {TMP.text=value.ToString();}
                }
            }
        }
    }
}