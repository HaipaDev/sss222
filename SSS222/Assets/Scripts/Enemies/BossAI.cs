using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BossAI : MonoBehaviour{
    public List<BossPhaseInfo> phasesInfo;
    [ReadOnly]public int phase;
    Enemy en;
    void Start(){
        en=GetComponent<Enemy>();

        var b=GameRules.instance.bossInfo;
        phasesInfo=b.phasesInfo;

        en.name=b.name;
        en.type=b.type;
        en.health=b.healthStart;en.healthMax=b.healthMax;
        en.defense=phasesInfo[0].defense;
        en.size=phasesInfo[0].size;
        en.sprMatProps=phasesInfo[0].sprMatProps;
        en.shooting=false;

        en.randomizeWaveDeath=false;
        en.flyOff=false;
        en.killOnDash=false;
        en.destroyOut=false;

        en.xpAmnt=b.xpAmnt;
        en.xpChance=b.xpChance;
        en.accumulateXp=false;
        en.drops=b.drops;
    }
    void Update(){
        if(phase>=0){
            en.defense=phasesInfo[phase].defense;
            en.size=phasesInfo[phase].size;
            en.sprMatProps=phasesInfo[phase].sprMatProps;

            en.spr=phasesInfo[phase].anims[0].spr;//Animate later
        }

        if(en.name.Contains("Moon of Lunacy")){MoonOfLunacyAI();}
    }
    void MoonOfLunacyAI(){
        if(phase==0){
            if(en.health<=en.healthMax/2){ChangePhase(1);}
        }else if(phase==1){
            if(en.health<=0){}
        }
    }
    IEnumerator _phaseCor;
    public void ChangePhase(int p){if(phasesInfo.Count>p){if(_phaseCor==null){_phaseCor=ChangePhaseI(p);StartCoroutine(_phaseCor);}}else{Debug.LogError("Cant change to phase: "+p);}}
    public IEnumerator ChangePhaseI(int p){
        phase=-1;
        en.defense=-1;
        GetComponent<PointPathing>().enabled=false;
        yield return new WaitForSeconds(1f);
        GetComponent<PointPathing>().enabled=true;
        phase=p;
        _phaseCor=null;
    }
}