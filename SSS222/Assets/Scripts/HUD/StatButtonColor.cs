using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatButtonColor : MonoBehaviour{
    string valueName;
    int value;
    int valueReq;
    Button bt;
    TMPro.TextMeshProUGUI txt;

    UpgradeMenu upgradeMenu;
    GameSession gameSession;
    
    void Start(){
        //img=GetComponent<Image>();
        bt=GetComponent<Button>();
        txt=GetComponent<TMPro.TextMeshProUGUI>();
        upgradeMenu=FindObjectOfType<UpgradeMenu>();
        gameSession=FindObjectOfType<GameSession>();
    }

    void Update(){
        if((bt!=null&&bt.gameObject.name=="ButtonHP")||(txt!=null&&txt.GetComponent<ValueDisplay>().value=="lvl_hp")){valueName="maxHealth_UpgradesLvl";}
        if((bt!=null&&bt.gameObject.name=="ButtonEnergy")||(txt!=null&&txt.GetComponent<ValueDisplay>().value=="lvl_energy")){valueName="maxEnergy_UpgradesLvl";}
        if((bt!=null&&bt.gameObject.name=="ButtonSpeed")||(txt!=null&&txt.GetComponent<ValueDisplay>().value=="lvl_speed")){valueName="speed_UpgradesLvl";}
        if((bt!=null&&bt.gameObject.name=="ButtonHpReg")||(txt!=null&&txt.GetComponent<ValueDisplay>().value=="lvl_hpRegen")){valueName="hpRegen_UpgradesLvl";}
        if((bt!=null&&bt.gameObject.name=="ButtonEnReg")||(txt!=null&&txt.GetComponent<ValueDisplay>().value=="lvl_enRegen")){valueName="enRegen_UpgradesLvl";}
        if((bt!=null&&bt.gameObject.name=="ButtonLuck")||(txt!=null&&txt.GetComponent<ValueDisplay>().value=="lvl_luck")){valueName="luck_UpgradesLvl";}

        if(valueName!=null)value=(int)upgradeMenu.GetType().GetField(valueName).GetValue(upgradeMenu);
        else{Debug.LogError(gameObject+"Value name empty");}
        valueReq=(int)upgradeMenu.GetType().GetField("total_UpgradesLvl").GetValue(upgradeMenu);
        if(valueReq<value||(valueReq==value&&value==0)){
            if(txt!=null)txt.color=Color.red;
            if(bt!=null){var co=bt.colors;
            co.normalColor=Color.red;
            bt.colors=co;
            var go=bt.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            go.color=Color.red;
            }
        /*}else if(gameSession.cores==){
            if(bt!=null){var co=bt.colors;
            co.normalColor=Color.red;
            bt.colors=co;
            var go=bt.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            go.color=Color.red;
            }*/
        }else{
            if(txt!=null)txt.color=Color.white;
            if(bt!=null){var co=bt.colors;
            co.normalColor=new Color(79,169,107);
            bt.colors=co;
            var go=bt.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            go.color=new Color(49,188,80);
            }
        }
    }
}
