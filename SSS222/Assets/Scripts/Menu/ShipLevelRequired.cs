using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipLevelRequired : MonoBehaviour{
    //[SerializeField] 
    TMPro.TextMeshProUGUI textObj;
    [SerializeField] string valueName;
    [SerializeField] int value;
    [SerializeField] string txt="LVL ";
    void Start(){
        var i=GameRules.instance;
        if(i!=null){
            value=(int)UpgradeMenu.instance.GetType().GetField(valueName).GetValue(UpgradeMenu.instance);
        }

        textObj=this.gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        textObj.text=txt+value;
    }
    void Update(){
        if(UpgradeMenu.instance.total_UpgradesLvl>=value||!GameSession.instance.levelingOn){this.gameObject.SetActive(false);}
    }
}
