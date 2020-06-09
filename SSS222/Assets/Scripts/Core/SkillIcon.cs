using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour{
    [SerializeField] GameObject parent;
    int ID;
    SkillSlotID[] skills;
    Sprite sprite;
    void Start(){
        skills=FindObjectOfType<PlayerSkills>().skills;
    }
    void Update(){
        ID=parent.GetComponent<SkillButtons>().ID;
        if(ID!=-1)sprite=skills[ID].sprite;
        GetComponent<Image>().sprite=sprite;
    }
}
