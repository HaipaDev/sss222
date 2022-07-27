using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BossAI : MonoBehaviour{
    public List<BossPhaseInfo> phasesInfo;
    [ReadOnly]public int phase;
    Enemy en;
    void Start(){
        phase=-1;
        en=GetComponent<Enemy>();
        en.size=Vector2.zero;
        var b=GameRules.instance.bossInfo;
        phasesInfo=b.phasesInfo;

        en.name=b.name;
        en.type=b.type;
        en.health=b.healthStart;en.healthMax=b.healthMax;
        en.defense=phasesInfo[0].defense;
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

        ChangePhase(0);
        //Spawn FX
        /*AudioManager.instance.Play(phasesInfo[0].audioAsset);
        GameAssets.instance.VFX(phasesInfo[0].vfxAsset,transform.position);
        Shake.instance.CamShake(phasesInfo[0].camShakeStrength,phasesInfo[0].camShakeSpeed);*/
    }
    void Update(){
        if(phase==-1&&GameRules.instance.bossInfo.scaleUpOnSpawn){
            var scaleUpSpeed=phasesInfo[0].delay/10f*Time.deltaTime;if(Vector2.Distance(en.size,phasesInfo[0].size)>scaleUpSpeed){en.spr=phasesInfo[0].anims[0].spr;en.size+=new Vector2(scaleUpSpeed,scaleUpSpeed);}
        }
        if(phase>=0){
            en.defense=phasesInfo[phase].defense;
            en.size=phasesInfo[phase].size;
            en.sprMatProps=phasesInfo[phase].sprMatProps;

            en.spr=phasesInfo[phase].anims[0].spr;//Animate later
        }

        if(en.name.Contains("Moon of Lunacy")){MoonOfLunacyAI();}
    }
    public void Die(){StartCoroutine(DieI());}
    IEnumerator DieI(){
        phase=-1;
        en.defense=-1;
        GetComponent<PointPathing>().enabled=false;
        Jukebox.instance.SetMusic(null);
        AudioManager.instance.Play(GameRules.instance.bossInfo.preDeathAudio);
        GameAssets.instance.VFX(GameRules.instance.bossInfo.preDeathVFX,transform.position,3f);
        yield return new WaitForSeconds(GameRules.instance.bossInfo.deathLength);
        GameAssets.instance.VFX(GameRules.instance.bossInfo.deathVFX,transform.position,3f);
        Shake.instance.CamShake(GameRules.instance.bossInfo.deathShakeStrength,GameRules.instance.bossInfo.deathShakeSpeed);
        Destroy(gameObject);
    }
    /*Coroutine anim;int iAnim=0;Sprite animSpr;
    IEnumerator AnimateBoss(SimpleAnim anim){Sprite spr;
        if(anim.delay>0){yield return new WaitForSeconds(anim.delay);}
        else{yield return new WaitForSeconds(anim[iAnim].delay);}
        spr=anim.animVals[iAnim].spr;
        if(iAnim==skin.animVals.Count-1)iAnim=0;
        if(iAnim<skin.animVals.Count)iAnim++;
        animSpr=spr;
        if(skinName==skin.name)anim=StartCoroutine(AnimateBoss(skin));
        else{if(anim!=null)StopCoroutine(anim);anim=null;iAnim=0;}
    }*/
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
        AudioManager.instance.Play(phasesInfo[p].audioAsset);
        Jukebox.instance.PauseFor(phasesInfo[p].delay);
        yield return new WaitForSeconds(phasesInfo[p].delay);
        if(p==0){Jukebox.instance.SetMusic(GameRules.instance.bossInfo.ost);if(SaveSerial.instance.settingsData.bossVolumeTurnUp){GameSession.instance._preBossMusicVolume=SaveSerial.instance.settingsData.musicVolume;SaveSerial.instance.settingsData.musicVolume=1f;}}
        GameAssets.instance.VFX(phasesInfo[p].vfxAsset,transform.position,3f);
        Shake.instance.CamShake(phasesInfo[p].camShakeStrength,phasesInfo[p].camShakeSpeed);
        GetComponent<PointPathing>().enabled=true;
        phase=p;
        _phaseCor=null;
    }
}