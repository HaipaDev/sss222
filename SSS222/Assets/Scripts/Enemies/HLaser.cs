using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HLaser : MonoBehaviour{
    [SerializeField] int stage=0;
    [SerializeField] float timerExcl=0.8f;
    [SerializeField] float timerHlaserCharging=1f;
    [SerializeField] float timerHlaser=3.3f;
    public float timer=-4;
    void Awake(){
        DisableAllChildren();
        transform.GetChild(stage).gameObject.SetActive(true);
        if(stage==0){timer=timerExcl;}
    }
    void Update(){
    if(timer>0)timer-=Time.deltaTime;
    if(timer<=0&&timer>-4){
        DisableAllChildren();
        if(stage<2){
            transform.GetChild(stage+1).gameObject.SetActive(true);
            stage++;
        }else if(stage==2){stage=3;}
        if(stage==1){timer=timerHlaserCharging;}
        else if(stage==2){timer=timerHlaser;}
        else if(stage==3){Destroy(gameObject);}
    }
    }
    void DisableAllChildren(){foreach(Transform t in transform){t.gameObject.SetActive(false);}}
}
