using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_DmgPhaseFreq:MonoBehaviour{
    public bool firstDone;public float phaseFreqFirst=1f;public float phaseFreq=0.38f;public float phaseTimer=-4;
    public int phaseCountLimit=0;public int phaseCount=0;
    public string soundPhase;
    public void ResetTimer(){firstDone=false;phaseTimer=phaseTimer=-4;phaseCount=0;}
    public void SetTimer(){phaseCount++;if(phaseTimer==-4){if(!firstDone){phaseTimer=phaseFreqFirst;firstDone=true;}else{phaseTimer=phaseFreq;}}}
    public void Update(){   if(!GameSession.GlobalTimeIsPaused){
        if(phaseFreqFirst==0){firstDone=true;}
        if(phaseTimer>0){phaseTimer-=Time.deltaTime;}
        else if(phaseTimer<=0&&phaseTimer!=-4){if(!String.IsNullOrEmpty(soundPhase))AudioManager.instance.Play(soundPhase);phaseTimer=-4;}
    }}
}