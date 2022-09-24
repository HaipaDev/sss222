using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HLaser : MonoBehaviour{
    [SerializeField] bool vlaser=true;
    [SerializeField] float timerWarn=0.8f;
    [SerializeField] float timerCharging=1f;
    [SerializeField] float timerStay=3.3f;
    [DisableInEditorMode] public int stage=0;
    [DisableInEditorMode] public float timer=-4;
    void Awake(){
        var i=GameRules.instance;
        if(i!=null){
            var e=i.vlaserSettings;
            if(!vlaser)e=i.hlaserSettings;
            timerWarn=e.timerWarn;
            timerCharging=e.timerCharging;
            timerStay=e.timerStay;
            if(e.chargingAnimation!=null){transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController=e.chargingAnimation;}else{Debug.LogWarning(gameObject.name+" chargingAnimation not assigned in "+i+"."+e);}
            if(e.hlaserAnimation!=null){transform.GetChild(2).GetComponent<Animator>().runtimeAnimatorController=e.hlaserAnimation;}else{Debug.LogWarning(gameObject.name+" hlaserAnimation not assigned in "+i+"."+e);}
        }
        
        DisableAllChildren();
        transform.GetChild(0).GetComponent<Canvas>().worldCamera=GameObject.Find("UICamera").GetComponent<Camera>();
        transform.GetChild(stage).gameObject.SetActive(true);
        if(stage==0){timer=timerWarn;}
    }
    void Update(){  if(!GameManager.GlobalTimeIsPaused){
        if(timer>0)timer-=Time.deltaTime;
        if(timer<=0&&timer>-4){
            DisableAllChildren();
            if(stage<2){
                transform.GetChild(stage+1).gameObject.SetActive(true);
                stage++;
            }else if(stage==2){stage=3;}
            if(stage==1){timer=timerCharging;}
            else if(stage==2){timer=timerStay;}
            else if(stage==3){Destroy(gameObject);}
        }
    }}
    void DisableAllChildren(){foreach(Transform t in transform){t.gameObject.SetActive(false);}}
}
