using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipLevelRequired : MonoBehaviour{
    [SerializeField] public int value;
    
    GameObject textObj;
    PlayerModules pmodules;
    void Start(){
        textObj=transform.GetChild(0).gameObject;
        pmodules=Player.instance.GetComponent<PlayerModules>();
    }
    void Update(){
        if(textObj!=null)textObj.GetComponent<TextMeshProUGUI>().text="Lvl "+value;
        if(pmodules.shipLvl>=value||!GameRules.instance.levelingOn){Switch();}
    }
    public void Switch(bool on=false){
        GetComponent<Image>().enabled=on;
        if(textObj!=null)textObj.GetComponent<TextMeshProUGUI>().enabled=on;
    }
}
