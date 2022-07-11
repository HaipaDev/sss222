using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class SkillButtons : MonoBehaviour{
    public int ID;
    [SerializeField] Image iconImg;
    PlayerModules pmodules;
    void Start(){
        pmodules=Player.instance.GetComponent<PlayerModules>();
    }

    void Update(){
        if(pmodules!=null){
            if(pmodules.skillsSlots!=null){if(pmodules.skillsSlots.Capacity>0){
                if(ID>=0&&pmodules.skillsSlots.Capacity>ID){
                if(pmodules.skillsSlots[ID]!=null){
                        if(pmodules.skillsSlots[ID]!=""){GameAssets.SetActiveAllChildren(transform,true);iconImg.sprite=pmodules.GetSkillProperties(pmodules.skillsSlots[ID]).item.sprite;}
                        else{GameAssets.SetActiveAllChildren(transform,false);}
                    }else{GameAssets.SetActiveAllChildren(transform,false);}
                }else{GameAssets.SetActiveAllChildren(transform,false);}
            }}
        }
    }
}
