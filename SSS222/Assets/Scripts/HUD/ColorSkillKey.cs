using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSkillKey : MonoBehaviour{
    [SerializeField] int ID;
    [SerializeField] skillKeyBind key;
    Player player;
    SkillSlotID[] skills;
    skillKeyBind[] skillsBinds;
    Color color;
    Image spr;
    void Start(){
        player=FindObjectOfType<Player>();
        skills=player.GetComponent<PlayerSkills>().skills;
        skillsBinds=player.GetComponent<PlayerSkills>().skillsBinds;
        spr=GetComponent<Image>();
    }

    void Update(){
        if(skillsBinds[ID]==key){color=Color.green;}
        else{color=Color.white;}
        spr.color=color;
    }
}
