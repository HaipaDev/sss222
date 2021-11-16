using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum skillKeyBind{
    Disabled, 
    Q, 
    E
    };
[CreateAssetMenu(menuName = "Skill Slot ID")]
public class SkillSlotID : ScriptableObject{
    [HeaderAttribute("Properties")]
    [SerializeField] public string skillName = "";
    [SerializeField] public Sprite sprite;
    //[SerializeField] public string pwrupName = "";
    [SerializeField] public int ID;
    [SerializeField] public costType costType;
    [SerializeField] public costTypeProperties costTypeProperties;
    [SerializeField] public float cooldown;

    [ContextMenu("ValidateCost")]void VaildateCost(){
        if(costType==costType.energy){costTypeProperties=new costTypeEnergy();}
        if(costType==costType.ammo){costTypeProperties=new costTypeAmmo();}
        if(costType==costType.crystalAmmo){costTypeProperties=new costTypeCrystalAmmo();}
        if(costType==costType.blackEnergy){costTypeProperties=new costTypeBlackEnergy();}
    }
}