using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Skill Properties")]
public class SkillProperties : ScriptableObject{
    [HeaderAttribute("Properties")]
    [SerializeField] public string skillName = "";
    [SerializeField] public Sprite sprite;
    [SerializeField] public costType costType;
    [SerializeField] public costTypeProperties costTypeProperties;
    [SerializeField] public float cooldown;
    [SerializeField] public int lvlReq;
    [SerializeField] public int coreCost;

    [ContextMenu("ValidateCost")]void VaildateCost(){
        if(costType==costType.energy){costTypeProperties=new costTypeEnergy();}
        if(costType==costType.ammo){costTypeProperties=new costTypeAmmo();}
        if(costType==costType.crystalAmmo){costTypeProperties=new costTypeCrystalAmmo();}
        if(costType==costType.blackEnergy){costTypeProperties=new costTypeBlackEnergy();}
    }
}