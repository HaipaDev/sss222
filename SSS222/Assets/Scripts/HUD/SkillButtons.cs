using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButtons : MonoBehaviour{
    /*[SerializeField] skillKeyBind keyBind;
    [SerializeField]skillKeyBind[] skillsBinds;
    [SerializeField]Skill[] skills;
    [SerializeField] SkillButtons[] buttons;*/
    public int ID;
    bool correct;
    //public bool on;
    void Start(){
        StartCoroutine(SetSkills());
    }
    IEnumerator SetSkills(){
        yield return new WaitForSecondsRealtime(0.2f);
        /*if(FindObjectOfType<PlayerModules>()!=null){
        if(FindObjectOfType<PlayerModules>().skillsBinds!=null)skillsBinds=FindObjectOfType<PlayerModules>().skillsBinds;
        if(FindObjectOfType<PlayerModules>().skills!=null)skills=FindObjectOfType<PlayerModules>().skills;
        }*/
    }

    void Update(){
        /*if(skillsBinds.Length>0)for(var i=0;i<skillsBinds.Length;i++){
            if(skillsBinds[i]==keyBind){ID=i;GameAssets.SetActiveAllChildren(transform,true);foreach(SkillButtons button in buttons){if(button.ID==this.ID && button!=this){button.ID=-1;}}}
            else if(ID==-1||(ID!=-1&&skillsBinds[ID]==skillKeyBind.Disabled)){GameAssets.SetActiveAllChildren(transform,false);}
        }*/
    }
}
