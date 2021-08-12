using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour{
    [SerializeField] GameObject parent;
    [SerializeField]int ID;
    [SerializeField]Skill[] skills;
    Sprite sprite;
    Image img;
    void Start(){
        img=GetComponent<Image>();
        parent=transform.parent.parent.gameObject;
        StartCoroutine(SetSkills());
    }
    IEnumerator SetSkills(){
        yield return new WaitForSecondsRealtime(0.2f);
        if(FindObjectOfType<PlayerSkills>()!=null)skills=FindObjectOfType<PlayerSkills>().skills;
    }
    void Update(){
        ID=parent.GetComponent<SkillButtons>().ID;
        if(ID!=-1)sprite=skills[ID].item.sprite;
        img.sprite=sprite;
    }
}
