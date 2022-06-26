using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour{
    [SerializeField] GameObject parent;
    [SerializeField]int ID;
    Sprite sprite;
    Image img;
    void Start(){
        img=GetComponent<Image>();
        parent=transform.parent.parent.gameObject;
    }
    void Update(){
        ID=parent.GetComponent<SkillButtons>().ID;
        if(ID!=-1)sprite=GameRules.instance.skillsPlayer[ID].sprite;
        img.sprite=sprite;
    }
}
