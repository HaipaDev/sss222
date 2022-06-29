using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableChildOnValue : MonoBehaviour{
    [SerializeField] public string valueName;
    [SerializeField] public float valueReq=1;
    public float value;
    Image image;
    PlayerModules pmodules;
    void Start(){
        image=GetComponent<Image>();
        pmodules=Player.instance.GetComponent<PlayerModules>();
        //valueReq=
    }

    void Update(){
        if(valueName.Contains("cooldownSkill_")){var _v=pmodules.GetSkillFromID(int.Parse(valueName.Split('_')[1]));if(_v!=null){value=_v.cooldown;}else{value=0;}}
        if(value>=valueReq){
            SetActiveAllChildren(transform,true);
        }else{SetActiveAllChildren(transform,false);}
    }

    void SetActiveAllChildren(Transform transform, bool value){
        foreach (Transform child in transform){
            child.gameObject.SetActive(value);
            SetActiveAllChildren(child, value);
        }
    }
}
