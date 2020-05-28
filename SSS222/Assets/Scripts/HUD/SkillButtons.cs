using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButtons : MonoBehaviour{
    [SerializeField] skillKeyBind keyBind;
    Player player;
    PlayerSkills pskills;
    skillKeyBind[] skillsBinds;
    //public bool on;
    void Start(){
        player=FindObjectOfType<Player>();
        pskills=player.GetComponent<PlayerSkills>();
        skillsBinds=pskills.skillsBinds;
    }

    void Update(){
        foreach(skillKeyBind skill in skillsBinds){
            if(skill==keyBind){SetActiveAllChildren(transform,true);}
            else{SetActiveAllChildren(transform,false);}
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
