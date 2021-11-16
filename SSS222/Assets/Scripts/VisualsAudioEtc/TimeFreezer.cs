using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFreezer : MonoBehaviour{
    public static TimeFreezer instance;
    float dur;
    bool _isFrozen=false;
    void Start(){
        instance=this;
    }

    void Update(){
        if(_pendingFreezeDuration>0&&!_isFrozen){
            StartCoroutine(DoFreeze());
        }
    }
    float _pendingFreezeDuration=0f;
    public void Freeze(float dur){
        _pendingFreezeDuration=dur;
        this.dur=dur;
    }
    IEnumerator DoFreeze(){
        _isFrozen=true;
        var ogTime=GameSession.instance.gameSpeed;
        var ogChanged=GameSession.instance.speedChanged;
        GameSession.instance.speedChanged=true;
        GameSession.instance.gameSpeed=0f;
        
        yield return new WaitForSecondsRealtime(dur);
        
        GameSession.instance.speedChanged=ogChanged;
        GameSession.instance.gameSpeed=ogTime;
        _pendingFreezeDuration=0;
        _isFrozen=false;
    }
}
