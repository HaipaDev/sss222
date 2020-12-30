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
    /*[HeaderAttribute("Displays")]
    [SerializeField] GameObject display1;
    [SerializeField] GameObject display2;*/
    //[SerializeField] GameObject display3;
    Player player;
    Image spr;
    TMPro.TextMeshProUGUI TMP;
    /*[HeaderAttribute("Sprites")]
    [SerializeField] Sprite laserSprite;
    [SerializeField] Sprite laser2Sprite;
    [SerializeField] Sprite laser3Sprite;
    [SerializeField] Sprite mlaserSprite;
    [SerializeField] Sprite lsaberSprite;
    [SerializeField] Sprite hrocketsSprite;
    [SerializeField] Sprite phaserSprite;
    [SerializeField] Sprite shadowbtSprite;
    [SerializeField] Sprite lclawsSprite;
    [SerializeField] Sprite qrocketsSprite;
    [SerializeField] Sprite procketsSprite;
    [SerializeField] Sprite cstreamSprite;

    [SerializeField] Sprite flipSprite;
    [SerializeField] Sprite gcloverSprite;
    [SerializeField] Sprite shadowSprite;
    [SerializeField] Sprite inverterSprite;
    [SerializeField] Sprite magnetSprite;
    [SerializeField] Sprite scalerSprite;
    [SerializeField] Sprite matrixSprite;
    [SerializeField] Sprite pmultiSprite;*/


    Color color;
    Color color2;
    Sprite sprite;
    Image bg;
    // Start is called before the first frame update
    void Start()
    {
        player=FindObjectOfType<Player>();
        //if(player!=null)pwrup = player.powerup;
        spr=GetComponent<Image>();
        //if(powerups!=true){
        if(powerups!=true||(player!=null&&player.weaponsLimited)){
        if(textObj==null){if(transform.GetComponentInChildren<TMPro.TextMeshProUGUI>()!=null)textObj=transform.GetComponentInChildren<TMPro.TextMeshProUGUI>().gameObject;}
        if(textObj!=null){TMP=textObj.GetComponent<TMPro.TextMeshProUGUI>();}
        bg=transform.GetChild(0).GetComponent<Image>();
        }
        color = spr.color;
        if(bg!=null)color2 = bg.color;
        if(GameRules.instance!=null){pwrup=GameRules.instance.powerupStarting;spr.sprite=GameAssets.instance.Spr(pwrup+"Pwrup");}//?
    }

    // Update is called once per frame
    void Update()
    {
        //if(player==null){player=FindObjectOfType<Player>();}
        spr.color = color;
        if(bg!=null)bg.color = color2;

        if(powerups==true){
            if(player!=null){
                pwrup = player.powerup;
                if(TMP!=null){
                    var timer=player.powerupTimer;
                    if((float)timer<10f&&(float)timer>=0){value=(float)System.Math.Round((float)timer, 1);TMP.characterSpacing=-25f;}
                    else if((float)timer>10f){value=(float)Mathf.RoundToInt((float)timer);TMP.characterSpacing=0f;}
                    else if((float)timer==-5){value=-5;}
                    //var value=System.Math.Round(timer, 1);

                    if (value<=0&&value>-5) {value = 0;}
                    if(value<=-5){TMP.text="∞";}
                    else {TMP.text=value.ToString();}
                }
                /*var sprr=this.GetType().GetField(pwrup+"Sprite").GetValue(this);
                this.GetType().GetField("sprite").SetValue(this,sprr);
                spr.sprite=sprite;*/
                if(/*pwrup!="laser"&&*/pwrup!="lsaberA"&&pwrup!="lclawsA"&&pwrup!="null"){if(GameAssets.instance!=null)spr.sprite=GameAssets.instance.Spr(pwrup+"Pwrup");}
                //else if(pwrup=="laser")spr.sprite=GameAssets.instance.Spr("laser");
                else if(pwrup=="lsaberA")spr.sprite=GameAssets.instance.Spr("lsaberPwrup");
                else if(pwrup=="lclawsA")spr.sprite=GameAssets.instance.Spr("lclawsPwrup");
                if(pwrup=="null")color.a=0;
                else color.a=1;
                /*if (pwrup=="laser2"){ spr.sprite = laser2Sprite; }
                else if(pwrup=="laser3"){ spr.sprite = laser3Sprite; }
                else if(pwrup=="mlaser"){ spr.sprite = mlaserSprite; }
                else if(pwrup=="lsaberA"){ spr.sprite = lsaberSprite; }
                else if(pwrup== "hrockets") { spr.sprite = hrocketsSprite; }
                else if(pwrup== "phaser") { spr.sprite = phaserSprite; }
                else if(pwrup== "shadowbt") { spr.sprite = shadowbtSprite; }
                else if(pwrup== "lclawsA") { spr.sprite = lclawsSprite; }
                else if(pwrup== "qrockets") { spr.sprite = qrocketsSprite; }
                else if(pwrup== "prockets") { spr.sprite = procketsSprite; }
                else if(pwrup== "cstream") { spr.sprite = cstreamSprite; }
                else { spr.sprite = laserSprite; }*/
            }
        }else{
            if(player!=null){
                if(player.statuses.Count>number){if(player.statuses[number]!=""){state=player.statuses[number];}else state="";}else state="";
                /*if(number==1){state=player.status1;}
                if(number==2){state=player.status2;}
                if(number==3){state=player.status3;}*/
                //var s=this.GetType().GetField(state+"Sprite").SetValue(this,false);
                //spr.sprite=s;
                if (state == "" || state == null){
                    Destroy(gameObject);
                    /*color.a = 0f;
                    color2.a = 0f;
                    TMP.text = "";*/
                }else{
                    color.a = 1f;
                    color2.a = 130/255f;
                    /*var sprr=this.GetType().GetField(state+"Sprite").GetValue(this);
                    this.GetType().GetField("sprite").SetValue(this,sprr);
                    spr.sprite=sprite;*/
                    if(GameAssets.instance!=null)spr.sprite=GameAssets.instance.Spr(state+"Pwrup");
                    var timer=player.GetType().GetField(state+"Timer").GetValue(player);
                    if((float)timer<10f&&(float)timer>=0){value=(float)System.Math.Round((float)timer, 1);TMP.characterSpacing=-25f;}
                    else if((float)timer>10f){value=(float)Mathf.RoundToInt((float)timer);TMP.characterSpacing=0f;}
                    else if((float)timer==-5){value=-5;}
                    //var value=System.Math.Round(timer, 1);

                    if (value<=0&&value>-5) {value = 0;}
                    if(value<=-5){TMP.text="∞";}
                    else {TMP.text=value.ToString();}
                }
            }
            #region//Old system
            /*
            //if(player.gclover==true && (state=="" || state==null)){ spr.sprite = gcloverSprite; state = "gclover"; }
            //if(player.flip==true && (state=="" || state==null)){ spr.sprite = flipSprite; state = "flip"; }
            if (textObj != null && player!=null)
            {
                //if (player.gclover == true){ TMP.text = player.gcloverTimer.ToString(); }
                //if (player.flip == true){ TMP.text = player.flipTimer.ToString(); }
                //if(display1!=null)if(display1.GetComponent<PowerupDisplay>().state!="gclover"){ }

                GameObject[] displays;
                displays = GameObject.FindGameObjectsWithTag("GameController");
                //foreach (GameObject display in displays){
                var iP = 1; var iM = 1;
                for (var i = 1; i < displays.Length; i += 1){
                    if(i>displays.Length-(displays.Length-1)){ iP = i-1; }
                    else if(i<=1){ iP = i+1; }
                    else{ iP = i + 1; }

                    if (i <= 1) { iM = 1; }
                    //else if(i>=displays.Length-1){ iM = i - 1; }
                    else { iM = i-1; }

                    //for(var iP = 2; iP<displays.Length; iP+=1){
                    //for (var iM = 0; iM < displays.Length; iM += 1){
                    //var iP = 2;
                    if (number == i){
                        if ((displays[i].GetComponent<PowerupDisplay>().state == "" || displays[i].GetComponent<PowerupDisplay>().state==null)){
                            if (((displays[iP].GetComponent<PowerupDisplay>().state!= "gclover") && (displays[iM].GetComponent<PowerupDisplay>().state != "gclover") && displays[iM-1].GetComponent<PowerupDisplay>().state != "gclover"))
                            {
                                if (player.gclover == true){state = "gclover";}//else { state = ""; }
                            }//else { state = ""; }
                            if (((displays[iP].GetComponent<PowerupDisplay>().state != "flip") && (displays[iM].GetComponent<PowerupDisplay>().state != "flip") && displays[iM - 1].GetComponent<PowerupDisplay>().state != "flip"))
                            {
                                if (player.flip == true){state = "flip";}//else { state = ""; }
                            }//else { state = ""; }
                            if (((displays[iP].GetComponent<PowerupDisplay>().state != "shadow") && (displays[iM].GetComponent<PowerupDisplay>().state != "shadow") && displays[iM - 1].GetComponent<PowerupDisplay>().state != "shadow"))
                            {
                                if (player.shadow == true){state = "shadow"; }//else { state = ""; }
                            }//else { state = ""; }
                            if (((displays[iP].GetComponent<PowerupDisplay>().state != "inverter") && (displays[iM].GetComponent<PowerupDisplay>().state != "inverter") && displays[iM - 1].GetComponent<PowerupDisplay>().state != "inverter"))
                            {
                                if (player.inverter == true){state = "inverted"; }//else { state = ""; }
                            }//else { state = ""; }
                            if (((displays[iP].GetComponent<PowerupDisplay>().state != "magnet") && (displays[iM].GetComponent<PowerupDisplay>().state != "magnet") && displays[iM - 1].GetComponent<PowerupDisplay>().state != "magnet"))
                            {
                                if (player.magnet == true){state = "magnet"; }//else { state = ""; }
                            }//else { state = ""; }
                            if (((displays[iP].GetComponent<PowerupDisplay>().state != "scaler") && (displays[iM].GetComponent<PowerupDisplay>().state != "scaler") && displays[iM - 1].GetComponent<PowerupDisplay>().state != "scaler"))
                            {
                                if (player.scaler == true){state = "scaler"; }//else { state = ""; }
                            }//else { state = ""; }
                            if (((displays[iP].GetComponent<PowerupDisplay>().state != "matrix") && (displays[iM].GetComponent<PowerupDisplay>().state != "matrix") && displays[iM - 1].GetComponent<PowerupDisplay>().state != "matrix"))
                            {
                                if (player.matrix == true){state = "matrix"; }//else { state = ""; }
                            }//else { state = ""; }
                            if (((displays[iP].GetComponent<PowerupDisplay>().state != "pmulti") && (displays[iM].GetComponent<PowerupDisplay>().state != "pmulti") && displays[iM - 1].GetComponent<PowerupDisplay>().state != "pmulti"))
                            {
                                if (player.pmultiTimer > 0){state = "pmulti"; }//else { state = ""; }
                            }//else { state = ""; }
                        }
                    }
                    //if(player.gclover!=true && player.flip!=true && player.shadow!=true){ state = ""; }
                    if (state == "" || state == null){
                        color.a = 0f;
                        TMP.text = "";
                    }else{
                        color.a = 1f;
                        if (state=="gclover"){
                            spr.sprite = gcloverSprite;
                            var value = System.Math.Round(player.GetGCloverTimer(), 1);

                            if (value <= -1) { value = 0; }
                            else { TMP.text = value.ToString(); }

                            if (player.gclover != true) state = "";
                        }else if(state=="flip"){
                            spr.sprite = flipSprite;
                            var value = System.Math.Round(player.GetFlipTimer(), 1);

                            if (value <= -1) { value = 0; }
                            else { TMP.text = value.ToString(); }

                            if (player.flip != true) state = "";
                        }else if(state=="shadow"){
                            spr.sprite = shadowSprite;
                            var value = System.Math.Round(player.GetShadowTimer(), 1);

                            if (value <= -1) { value = 0; }
                            else { TMP.text = value.ToString(); }

                            if (player.shadow != true) state = "";
                        }else if(state=="inverted"){
                            spr.sprite = inverterSprite;
                            var value = System.Math.Round(player.GetInverterTimer(), 1);

                            if (value <= -1) { value = 0; }
                            else { TMP.text = value.ToString(); }

                            if (player.inverter!= true) state = "";
                        }else if(state=="magnet"){
                            spr.sprite = magnetSprite;
                            var value = System.Math.Round(player.GetMagnetTimer(), 1);

                            if (value <= -1) { value = 0; }
                            else { TMP.text = value.ToString(); }

                            if (player.magnet!= true) state = "";
                        }else if(state=="scaler"){
                            spr.sprite = scalerSprite;
                            var value = System.Math.Round(player.GetScalerTimer(), 1);

                            if (value <= -1) { value = 0; }
                            else { TMP.text = value.ToString(); }

                            if (player.scaler!= true) state = "";
                        }else if(state=="matrix"){
                            spr.sprite = matrixSprite;
                            var value = System.Math.Round(player.GetMatrixTimer(), 1);

                            if (value <= -1) { value = 0; }
                            else { TMP.text = value.ToString(); }

                            if (player.matrix!= true) state = "";
                        }else if(state=="pmulti"){
                            spr.sprite = pmultiSprite;
                            var value = System.Math.Round(player.GetPMultiTimer(), 1);

                            if (value <= -1) { value = 0; }
                            else { TMP.text = value.ToString(); }

                            if (player.pmultiTimer <0) state = "";
                        }
                    }
                    //}
                    //}
                }
                //Debug.Log(displays.Length);
                //}
            }
            */
            #endregion
        }
        
    }
}