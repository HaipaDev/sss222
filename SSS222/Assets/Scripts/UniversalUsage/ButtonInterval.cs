using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInterval : MonoBehaviour{
    [SerializeField] float timer=0.5f;
    Button button;
    float timerMax;
    void Awake() {
        button = GetComponent<Button>();
        timerMax=timer;
    }
    void OnEnable(){
        button.interactable = false;
    }
    void OnDisable() {
        timer=timerMax;
    }

    
    void Update(){
        if(timer>0){timer -= Time.unscaledDeltaTime; }
        else{ button.interactable=true; }
    }
}
