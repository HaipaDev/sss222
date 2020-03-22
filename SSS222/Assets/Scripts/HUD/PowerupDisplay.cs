using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupDisplay : MonoBehaviour{
    Player player;
    string pwrup;
    SpriteRenderer spr;
    [SerializeField] int number;
    [SerializeField] bool powerups = true;
    [SerializeField] GameObject textObj;
    public string state="";
    /*[HeaderAttribute("Displays")]
    [SerializeField] GameObject display1;
    [SerializeField] GameObject display2;*/
    //[SerializeField] GameObject display3;
    TMPro.TextMeshProUGUI TMP;
    [HeaderAttribute("Sprites")]
    [SerializeField] Sprite laserSprite;
    [SerializeField] Sprite laser2Sprite;
    [SerializeField] Sprite laser3Sprite;
    [SerializeField] Sprite mlaserSprite;
    [SerializeField] Sprite lsaberSprite;
    [SerializeField] Sprite hrocketsSprite;
    [SerializeField] Sprite phaserSprite;

    [SerializeField] Sprite flipSprite;
    [SerializeField] Sprite gcloverSprite;
    [SerializeField] Sprite shadowSprite;

    

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        pwrup = player.GetComponent<Player>().powerup;
        spr = GetComponent<SpriteRenderer>();
        if(textObj!=null)TMP=textObj.GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (powerups==true){
            pwrup = player.GetComponent<Player>().powerup;
            if (pwrup=="laser2"){ spr.sprite = laser2Sprite; }
            else if(pwrup=="laser3"){ spr.sprite = laser3Sprite; }
            else if(pwrup=="mlaser"){ spr.sprite = mlaserSprite; }
            else if(pwrup=="lsaber"){ spr.sprite = lsaberSprite; }
            else if(pwrup== "hrockets") { spr.sprite = hrocketsSprite; }
            else if(pwrup== "phaser") { spr.sprite = phaserSprite; }
            else { spr.sprite = laserSprite; }
        }else{
            //if(player.gclover==true && (state=="" || state==null)){ spr.sprite = gcloverSprite; state = "gclover"; }
            //if(player.flip==true && (state=="" || state==null)){ spr.sprite = flipSprite; state = "flip"; }
            if (textObj != null)
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
                        }
                    }
                    //if(player.gclover!=true && player.flip!=true && player.shadow!=true){ state = ""; }
                    if(state=="gclover"){
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
                    }
                    else if(state=="shadow"){
                        spr.sprite = shadowSprite;
                        var value = System.Math.Round(player.GetShadowTimer(), 1);

                        if (value <= -1) { value = 0; }
                        else { TMP.text = value.ToString(); }

                        if (player.shadow != true) state = "";
                    }
                    //}
                    //}
                }
                //Debug.Log(displays.Length);
                //}
            }
        }
    }
}