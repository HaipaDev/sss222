using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipLevelRequired : MonoBehaviour{
    [SerializeField] public int value;
    [SerializeField] public bool expire;
    [SerializeField] public bool adventureData;
    
    GameObject textObj;
    PlayerModules pmodules;
    void Start(){
        textObj=transform.GetChild(0).gameObject;
        if(Player.instance!=null)pmodules=Player.instance.GetComponent<PlayerModules>();
        if(expire){Switch();}else{Switch(true);}
    }
    void OnEnable(){
        //if(expire&&value==0){Destroy(this.gameObject);}
    }
    void Update(){
        if(textObj!=null){var _txt="Lvl "+value;if(expire){_txt="Expired at Lvl "+value;}textObj.GetComponent<TextMeshProUGUI>().text=_txt;}
        if(pmodules!=null){if(pmodules.shipLvl>=value||!GameRules.instance.levelingOn){if(!expire){Switch();}else{Switch(true);}}}
        if(adventureData){if(SaveSerial.instance.advD.shipLvl>=value){if(!expire){Switch();}else{Switch(true);}}}
    }
    public void Switch(bool on=false){
        GetComponent<Image>().enabled=on;
        if(textObj!=null)textObj.GetComponent<TextMeshProUGUI>().enabled=on;
    }
}
