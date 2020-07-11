﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButtons : MonoBehaviour{
    [SerializeField] skillKeyBind keyBind;
    Player player;
    PlayerSkills pskills;
    skillKeyBind[] skillsBinds;
    SkillSlotID[] skills;
    [SerializeField] SkillButtons[] buttons;
    public int ID;
    bool correct;
    //public bool on;
    void Start(){
        player=FindObjectOfType<Player>();
        pskills=player.GetComponent<PlayerSkills>();
        skillsBinds=pskills.skillsBinds;
        skills=pskills.skills;
    }

    void Update(){
        /*foreach(SkillSlotID skill in skills){
            if(correct==true)ID=skill.ID;
        }
        foreach(skillKeyBind skill in skillsBinds){
            if(skill==keyBind){correct=true;SetActiveAllChildren(transform,true);}
            else if(skill==skillKeyBind.Disabled && ID!=skill.ID){SetActiveAllChildren(transform,false);}
        }*/
        for(var i=0;i<skillsBinds.Length;i++){
            if(skillsBinds[i]==keyBind){ID=i;SetActiveAllChildren(transform,true);foreach(SkillButtons button in buttons){if(button.ID==this.ID && button!=this){button.ID=-1;}}}
            else if(ID==-1||(ID!=-1&&skillsBinds[ID]==skillKeyBind.Disabled)){SetActiveAllChildren(transform,false);}
        }
    }

    private void SetActiveAllChildren(Transform transform, bool value)
     {
         foreach (Transform child in transform)
         {
             child.gameObject.SetActive(value);
 
             SetActiveAllChildren(child, value);
         }
     }
}