using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipLevelRequired : MonoBehaviour{
    TextMeshProUGUI textObj;
    [SerializeField] string valueName;
    [SerializeField] public int value;
    [SerializeField] string txt="LVL ";
    void Start(){
        var i=GameRules.instance;
        if(i!=null){
            if(!System.String.IsNullOrEmpty(valueName))value=(int)UpgradeMenu.instance.GetType().GetField(valueName).GetValue(UpgradeMenu.instance);
        }

        textObj=this.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        textObj.text=txt+value;
    }
    void Update(){
        if(UpgradeMenu.instance.total_UpgradesLvl>=value||!GameRules.instance.levelingOn){this.gameObject.SetActive(false);}
    }
}
