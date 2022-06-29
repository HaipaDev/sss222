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
            if(ID>=0&&ID<pmodules.skillsSlots.Capacity){
                if(pmodules.skillsSlots[ID]!=""){GameAssets.SetActiveAllChildren(transform,true);iconImg.sprite=pmodules.GetSkillProperties(pmodules.skillsSlots[ID]).item.sprite;}
                else{GameAssets.SetActiveAllChildren(transform,false);}
            }else{GameAssets.SetActiveAllChildren(transform,false);}
        }
    }
}
