using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_DmgPhaseFreq:MonoBehaviour{bool firstDone;public float phaseFreqFirst=1f;public float phaseFreq=0.38f;public float phaseTimer=-4;
    public void ResetTimer(){firstDone=false;phaseTimer=phaseTimer=-4;}
    public void SetTimer(){if(phaseTimer==-4){if(!firstDone){phaseTimer=phaseFreqFirst;firstDone=true;}else{phaseTimer=phaseFreq;}}}
    public void Update(){
        if(phaseFreqFirst==0){firstDone=true;}
        if(!GameSession.GlobalTimeIsPaused){if(phaseTimer>0){phaseTimer-=Time.deltaTime;}else{phaseTimer=-4;}}
    }
}