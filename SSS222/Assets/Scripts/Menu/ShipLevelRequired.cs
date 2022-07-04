using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipLevelRequired : MonoBehaviour{
    [SerializeField] public string valueName;
    [SerializeField] public int value;
    
    GameObject textObj;
    void Start(){
        textObj=transform.GetChild(0).gameObject;

        //var i=GameRules.instance;
        //if(i!=null){
            if(!System.String.IsNullOrEmpty(valueName))value=(int)UpgradeMenu.instance.GetType().GetField(valueName).GetValue(UpgradeMenu.instance);
        //}
    }
    void Update(){
        if(textObj!=null)textObj.GetComponent<TextMeshProUGUI>().text="Lvl "+value;
        if(UpgradeMenu.instance.total_UpgradesLvl>=value||!GameRules.instance.levelingOn){Switch();}
    }
    public void Switch(bool on=false){
        GetComponent<Image>().enabled=on;
        if(textObj!=null)textObj.GetComponent<TextMeshProUGUI>().enabled=on;
    }
}
