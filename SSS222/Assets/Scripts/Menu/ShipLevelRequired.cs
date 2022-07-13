using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipLevelRequired : MonoBehaviour{
    [SerializeField] public int value;
    [SerializeField] public bool adventureData;
    
    GameObject textObj;
    PlayerModules pmodules;
    void Start(){
        textObj=transform.GetChild(0).gameObject;
        if(Player.instance!=null)pmodules=Player.instance.GetComponent<PlayerModules>();
    }
    void Update(){
        if(textObj!=null)textObj.GetComponent<TextMeshProUGUI>().text="Lvl "+value;
        if(pmodules!=null){if(pmodules.shipLvl>=value||!GameRules.instance.levelingOn){Switch();}}
        if(adventureData){if(SaveSerial.instance.advD.shipLvl>=value){Switch();}}
    }
    public void Switch(bool on=false){
        GetComponent<Image>().enabled=on;
        if(textObj!=null)textObj.GetComponent<TextMeshProUGUI>().enabled=on;
    }
}
