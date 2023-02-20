using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tag_ModuleTxtUpgradeEquip : MonoBehaviour{
    [SerializeField] bool equip=true;
    public bool skill;
    public string value;
    public void UpdateTxt(){
        var pmodules=FindObjectOfType<PlayerModules>();
        var _txt=GetComponent<TextMeshProUGUI>().text;
        if(!skill){
            if(equip){var m_id=pmodules.moduleSlots.FindIndex(x=>x==value);if(m_id!=UpgradeMenu.instance.selectedModuleSlot){_txt="Equip";}/*else _txt="";}*/}
            else{var m=pmodules.modulesList.Find(x=>x.name==value);if(m.lvl==0){_txt="Unlock";}else if(m.lvl>0&&m.lvl<pmodules.GetModuleProperties(value).lvlVals.Count){_txt="Upgrade";}else{_txt="Max Lvl";}}
        }else{
            if(equip){var s_id=pmodules.skillsSlots.FindIndex(x=>x==value);if(s_id!=UpgradeMenu.instance.selectedSkillSlot){_txt="Equip";}/*else _txt="";}*/}     
            else{var s=pmodules.skillsList.Find(x=>x.name==value);if(s.lvl==0){_txt="Unlock";}else if(s.lvl>0&&s.lvl<pmodules.GetSkillProperties(value).lvlVals.Count){_txt="Upgrade";}else{_txt="Max Lvl";}}
        }
        GetComponent<TextMeshProUGUI>().text=_txt;
    }
}
