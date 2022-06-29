using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIcon : MonoBehaviour{
    [SerializeField] GameObject parent;
    [SerializeField]int ID;
    Sprite sprite;
    Image img;
    PlayerModules pmodules;
    void Start(){
        img=GetComponent<Image>();
        parent=transform.parent.parent.gameObject;
        pmodules=Player.instance.GetComponent<PlayerModules>();
    }
    void Update(){
        ID=parent.GetComponent<SkillButtons>().ID;
        if(ID!=-1)sprite=pmodules.GetSkillProperties(pmodules.skillsSlots[ID]).item.sprite;
        img.sprite=sprite;
    }
}
