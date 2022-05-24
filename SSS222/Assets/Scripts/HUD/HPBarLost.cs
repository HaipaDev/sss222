using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class HPBarLost : MonoBehaviour{
    [SerializeField]float timeToDrain=0.3f;
    [SerializeField]float drainSpeed=1f;
    [DisableInEditorMode][SerializeField]float currentVal;
    float timer=-4;
    void Start(){}
    void Update(){
        if(Player.instance!=null){GetComponent<Image>().fillAmount=(currentVal/Player.instance.healthMax);}
        else{GetComponent<Image>().fillAmount=0;}
        if(timer>0){timer-=Time.unscaledDeltaTime;}
        else{if(currentVal>Player.instance.health)currentVal-=drainSpeed;}
    }
    public void TriggerBar(){currentVal=Player.instance.GetComponent<PlayerCollider>()._LastHp();timer=timeToDrain;}
}
