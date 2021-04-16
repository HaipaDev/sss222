using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObject_GameRules : MonoBehaviour{
    [SerializeField] string valueName;
    [SerializeField] bool value;
    void Start(){
        value=(bool)GameRules.instance.GetType().GetField(valueName).GetValue(GameRules.instance);
        if(!value){gameObject.SetActive(false);}
    }

    void Update(){
        
    }
}
