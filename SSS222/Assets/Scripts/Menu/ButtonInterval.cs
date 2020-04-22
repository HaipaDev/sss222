using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInterval : MonoBehaviour{
    [SerializeField] float timer;
    Button button;
    void Start(){
        button = GetComponent<Button>();
        button.interactable = false;
    }

    
    void Update(){
        if(timer>0){timer -= Time.unscaledDeltaTime; }
        else{ button.interactable=true; }
    }
}
