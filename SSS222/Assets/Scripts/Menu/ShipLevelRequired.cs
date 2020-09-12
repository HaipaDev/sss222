using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipLevelRequired : MonoBehaviour{
    //[SerializeField] 
    TMPro.TextMeshProUGUI textObj;
    [SerializeField] int unlockableID;
    [SerializeField] int level;
    [SerializeField] string txt="LEVEL REQUIRED: ";
    UpgradeMenu upgradeMenu;
    void Start(){
        var i=GameRules.instance;
        if(i!=null){
            level=i.unlockableSkills[unlockableID];
        }

        upgradeMenu=FindObjectOfType<UpgradeMenu>();
        textObj=this.gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        textObj.text=txt+level;
        if(upgradeMenu.total_UpgradesLvl>=level){this.gameObject.SetActive(false);}
    }
    void Update(){
        if(upgradeMenu.total_UpgradesLvl>=level){this.gameObject.SetActive(false);}
    }
}
