using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour{
    [SerializeField] GameObject parent;
    int ID;
    Skill[] skills;
    Sprite sprite;
    void Start(){
        StartCoroutine(SetSkills());
    }
    IEnumerator SetSkills(){
        yield return new WaitForSeconds(0.1f);
        skills=FindObjectOfType<PlayerSkills>().skills;
    }
    void Update(){
        ID=parent.GetComponent<SkillButtons>().ID;
        if(ID!=-1)sprite=skills[ID].item.sprite;
        GetComponent<Image>().sprite=sprite;
    }
}
